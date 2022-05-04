using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KidQquest.Models;
using KidQquest.Params.PreviewParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PreviewController : Controller
{
    private readonly ILogger<PreviewController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;

    private string PathToStorage => Path.Combine(Environment.GetEnvironmentVariable("STORAGE_PATH"), "previews");

    public PreviewController(
        ILogger<PreviewController> logger,
        ApplicationContext context,
        IJwtService jwtService)
    {
        _logger = logger;
        _db = context;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<ActionResult<UploadPreviewResponse>> Upload(IFormFile uploadedFile, [FromHeader] string secretKey)
    {
        if (uploadedFile is null)
            return new JsonResult(new UploadPreviewResponse
            {
                Status = UploadPreviewResponseStatus.FileIsNull
            });

        if (secretKey != SecretKeyProvider.SecretKey)
            return new JsonResult(new UploadPreviewResponse
            {
                Status = UploadPreviewResponseStatus.InvalidSecretKey
            });

        var time = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
        if (!Directory.Exists(PathToStorage))
        {
            Directory.CreateDirectory(PathToStorage);
        }
        var path = Path.Combine(PathToStorage, $"{time}-" + uploadedFile.FileName);
        await using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await uploadedFile.CopyToAsync(fileStream);
        }

        var file = new PreviewModel
        {
            Name = uploadedFile.FileName,
            Path = path,
            DocType = uploadedFile.ContentType
        };
        _db.Previews.Add(file);
        await _db.SaveChangesAsync();

        return new JsonResult(new UploadPreviewResponse
        {
            Status = UploadPreviewResponseStatus.Ok,
            Id = file.Id
        });
    }

    [HttpGet]
    public IActionResult Download(int id, [FromHeader] string token)
    {
        if (token is null)
            return BadRequest();

        var userEmail = _jwtService.Decode(token);
        var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);

        if (user is null || user.Status != UserStatus.Active)
            return StatusCode(404);

        var fileModel = _db.Previews.FirstOrDefault(f => f.Id == id);
        if (fileModel is null)
            return StatusCode(404);

        var file = System.IO.File.OpenRead(fileModel.Path);
        return File(file, fileModel.DocType);
    }
}