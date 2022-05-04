using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.FactParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FactController : Controller
{
    private readonly ILogger<FactController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public FactController(
        ILogger<FactController> logger,
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
    public async Task<ActionResult<CreateFactResponse>> Create(
        [FromBody] CreateFactRequest request,
        [FromHeader] string secretKey)
    {
        CreateFactResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateFactResponse
            {
                Status = CreateFactResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.FirstOrDefault(e => e.Id == request.QuestionId);
        if (question is null)
        {
            response = new CreateFactResponse
            {
                Status = CreateFactResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        // var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        // if (preview is null)
        // {
        //     response = new CreateFactResponse
        //     {
        //         Status = CreateFactResponseStatus.InvalidPreviewId
        //     };
        //
        //     return new JsonResult(response);
        // }

        var factModel = _mapper.Map<FactModel>(request);
        _db.Facts.Add(factModel);
        await _db.SaveChangesAsync();

        response = new CreateFactResponse
        {
            Status = CreateFactResponseStatus.Ok,
            Id = factModel.Id
        };

        return new JsonResult(response);
    }

    [HttpPost]
    public async Task<ActionResult<UpdateFactResponse>> Update(
        [FromBody] UpdateFactRequest request,
        [FromHeader] string secretKey)
    {
        UpdateFactResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new UpdateFactResponse
            {
                Status = UpdateFactResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var fact = _db.Facts.FirstOrDefault(e => e.Id == request.Id);
        if (fact is null)
        {
            response = new UpdateFactResponse
            {
                Status = UpdateFactResponseStatus.InvalidFactId
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.FirstOrDefault(e => e.Id == request.QuestionId);
        if (question is null)
        {
            response = new UpdateFactResponse
            {
                Status = UpdateFactResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        // var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        // if (preview is null)
        // {
        //     response = new UpdateFactResponse
        //     {
        //         Status = UpdateFactResponseStatus.InvalidPreviewId
        //     };
        //
        //     return new JsonResult(response);
        // }

        _mapper.Map(request, fact);
        _db.Facts.Update(fact);
        await _db.SaveChangesAsync();

        response = new UpdateFactResponse
        {
            Status = UpdateFactResponseStatus.Ok
        };

        return new JsonResult(response);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteFactResponse>> Delete(
        [FromQuery] int id,
        [FromHeader] string secretKey)
    {
        DeleteFactResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteFactResponse
            {
                Status = DeleteFactResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var fact = _db.Facts.FirstOrDefault(e => e.Id == id);
        if (fact is null)
        {
            response = new DeleteFactResponse
            {
                Status = DeleteFactResponseStatus.InvalidFactId
            };

            return new JsonResult(response);
        }

        _db.Facts.Remove(fact);
        await _db.SaveChangesAsync();

        response = new DeleteFactResponse
        {
            Status = DeleteFactResponseStatus.Ok
        };

        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetFactByQuestionIdResponse>> GetByQuestionId(
        [FromHeader] int questionId,
        [FromHeader] string token)
    {
        GetFactByQuestionIdResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetFactByQuestionIdResponse
            {
                Status = GetFactByQuestionIdResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetFactByQuestionIdResponse
            {
                Status = GetFactByQuestionIdResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetFactByQuestionIdResponse
            {
                Status = GetFactByQuestionIdResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.Include(e => e.Fact).FirstOrDefault(e => e.Id == questionId);
        if (question is null)
        {
            response = new GetFactByQuestionIdResponse
            {
                Status = GetFactByQuestionIdResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        var fact = question.Fact;
        if (fact is null)
        {
            response = new GetFactByQuestionIdResponse
            {
                Status = GetFactByQuestionIdResponseStatus.FactIsNull
            };

            return new JsonResult(response);
        }

        response = new GetFactByQuestionIdResponse
        {
            Status = GetFactByQuestionIdResponseStatus.Ok,
            Fact = _mapper.Map<FactDto>(fact)
        };

        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetAllFactsResponse>> GetAll([FromHeader] string token)
    {
        GetAllFactsResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllFactsResponse
            {
                Status = GetAllFactsResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllFactsResponse
            {
                Status = GetAllFactsResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllFactsResponse
            {
                Status = GetAllFactsResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var factDtos = _db.Facts.ToArray().Select(e => _mapper.Map<FactDto>(e));


        response = new GetAllFactsResponse
        {
            Status = GetAllFactsResponseStatus.Ok,
            Facts = factDtos
        };

        return new JsonResult(response);
    }
}