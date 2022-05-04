using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.QuestionTypeParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class QuestionTypeController : Controller
{
    private readonly ILogger<QuestionTypeController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    
    public QuestionTypeController(
        ILogger<QuestionTypeController> logger,
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
    public async Task<ActionResult<CreateQuestionTypeResponse>> Create(
        [FromBody] CreateQuestionTypeRequest request, 
        [FromHeader] string secretKey)
    {
        CreateQuestionTypeResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateQuestionTypeResponse
            {
                Status = CreateQuestionTypeResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var questionTypeModel = _mapper.Map<QuestionTypeModel>(request);
        _db.QuestionTypes.Add(questionTypeModel);
        await _db.SaveChangesAsync();

        response = new CreateQuestionTypeResponse
        {
            Status = CreateQuestionTypeResponseStatus.Ok,
            Id = questionTypeModel.Id
        };

        return new JsonResult(response);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteQuestionTypeResponse>> Delete(
        [FromQuery] int id, 
        [FromHeader] string secretKey)
    {
        DeleteQuestionTypeResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteQuestionTypeResponse
            {
                Status = DeleteQuestionTypeResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var questionType = _db.QuestionTypes.FirstOrDefault(e => e.Id == id);
        if (questionType is null)
        {
            response = new DeleteQuestionTypeResponse
            {
                Status = DeleteQuestionTypeResponseStatus.InvalidQuestionTypeId
            };

            return new JsonResult(response);
        }
        _db.QuestionTypes.Remove(questionType);
        await _db.SaveChangesAsync();

        response = new DeleteQuestionTypeResponse
        {
            Status = DeleteQuestionTypeResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetByIdQuestionTypeResponse>> GetById(
        [FromHeader] int id, 
        [FromHeader] string token)
    {
        GetByIdQuestionTypeResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetByIdQuestionTypeResponse
            {
                Status = GetByIdQuestionTypeResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetByIdQuestionTypeResponse
            {
                Status = GetByIdQuestionTypeResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetByIdQuestionTypeResponse
            {
                Status = GetByIdQuestionTypeResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }
        
        var questionType = _db.QuestionTypes.FirstOrDefault(e => e.Id == id);
        if (questionType is null)
        {
            response = new GetByIdQuestionTypeResponse
            {
                Status = GetByIdQuestionTypeResponseStatus.InvalidQuestionTypeId
            };

            return new JsonResult(response);
        }
        

        response = new GetByIdQuestionTypeResponse
        {
            Status = GetByIdQuestionTypeResponseStatus.Ok,
            QuestionType = _mapper.Map<QuestionTypeDto>(questionType)
        };

        return new JsonResult(response);
    }
   
    [HttpGet]
    public async Task<ActionResult<GetAllQuestionTypeResponse>> GetAll([FromHeader] string token)
    {
        GetAllQuestionTypeResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllQuestionTypeResponse
            {
                Status = GetAllQuestionTypeResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllQuestionTypeResponse
            {
                Status = GetAllQuestionTypeResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllQuestionTypeResponse
            {
                Status = GetAllQuestionTypeResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }

        var questionTypeDtos = _db.QuestionTypes.ToArray().Select(e => _mapper.Map<QuestionTypeDto>(e));


        response = new GetAllQuestionTypeResponse
        {
            Status = GetAllQuestionTypeResponseStatus.Ok,
            QuestionTypes = questionTypeDtos
        };

        return new JsonResult(response);
    }
}