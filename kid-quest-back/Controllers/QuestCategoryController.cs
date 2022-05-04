using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.QuestCategoryParams;
using KidQquest.Params.QuestParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class QuestCategoryController : Controller
{
    private readonly ILogger<QuestCategoryController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    
    public QuestCategoryController(
        ILogger<QuestCategoryController> logger,
        ApplicationContext context,
        IJwtService jwtService, 
        IMapper mapper)
    {
        _logger = logger;
        _db = context;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CreateQuestCategoryResponse>> Create(
        [FromBody] CreateQuestCategoryRequest request, 
        [FromHeader] string secretKey)
    {
        CreateQuestCategoryResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateQuestCategoryResponse
            {
                Status = CreateQuestCategoryResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var questCategoryModel = _mapper.Map<QuestCategoryModel>(request);
        _db.QuestCategories.Add(questCategoryModel);
        await _db.SaveChangesAsync();

        response = new CreateQuestCategoryResponse
        {
            Status = CreateQuestCategoryResponseStatus.Ok,
            Id = questCategoryModel.Id
        };

        return new JsonResult(response);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteQuestCategoryResponse>> Delete(
        [FromQuery] int id, 
        [FromHeader] string secretKey)
    {
        DeleteQuestCategoryResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteQuestCategoryResponse
            {
                Status = DeleteQuestCategoryResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var questCategory = _db.QuestCategories.FirstOrDefault(e => e.Id == id);
        if (questCategory is null)
        {
            response = new DeleteQuestCategoryResponse
            {
                Status = DeleteQuestCategoryResponseStatus.InvalidQuestCategoryId
            };

            return new JsonResult(response);
        }
        _db.QuestCategories.Remove(questCategory);
        await _db.SaveChangesAsync();

        response = new DeleteQuestCategoryResponse
        {
            Status = DeleteQuestCategoryResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetByIdQuestCategoryResponse>> GetById(
        [FromHeader] int id, 
        [FromHeader] string token)
    {
        GetByIdQuestCategoryResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetByIdQuestCategoryResponse
            {
                Status = GetByIdQuestCategoryResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetByIdQuestCategoryResponse
            {
                Status = GetByIdQuestCategoryResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetByIdQuestCategoryResponse
            {
                Status = GetByIdQuestCategoryResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }
        
        var questCategory = _db.QuestCategories.FirstOrDefault(e => e.Id == id);
        if (questCategory is null)
        {
            response = new GetByIdQuestCategoryResponse
            {
                Status = GetByIdQuestCategoryResponseStatus.InvalidQuestCategoryId
            };

            return new JsonResult(response);
        }
        

        response = new GetByIdQuestCategoryResponse
        {
            Status = GetByIdQuestCategoryResponseStatus.Ok,
            QuestCategory = _mapper.Map<QuestCategoryDto>(questCategory)
        };

        return new JsonResult(response);
    }
   
    [HttpGet]
    public async Task<ActionResult<GetAllQuestCategoryResponse>> GetAll([FromHeader] string token)
    {
        GetAllQuestCategoryResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllQuestCategoryResponse
            {
                Status = GetAllQuestCategoryResponseStatus.InvalidToken
            };
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllQuestCategoryResponse
            {
                Status = GetAllQuestCategoryResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllQuestCategoryResponse
            {
                Status = GetAllQuestCategoryResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }

        var questCategoryDtos = _db.QuestCategories.ToArray().Select(e => _mapper.Map<QuestCategoryDto>(e));


        response = new GetAllQuestCategoryResponse
        {
            Status = GetAllQuestCategoryResponseStatus.Ok,
            QuestCategories = questCategoryDtos
        };

        return new JsonResult(response);
    }
}