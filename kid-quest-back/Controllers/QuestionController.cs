using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.QuestionParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class QuestionController : Controller
{
    private readonly ILogger<QuestionController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    
    public QuestionController(
        ILogger<QuestionController> logger,
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
    public async Task<ActionResult<CreateQuestionResponse>> Create(
        [FromBody] CreateQuestionRequest request, 
        [FromHeader] string secretKey)
    {
        CreateQuestionResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateQuestionResponse
            {
                Status = CreateQuestionResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == request.QuestId);
        if (quest is null)
        {
            response = new CreateQuestionResponse
            {
                Status = CreateQuestionResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        
        // var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        // if (preview is null)
        // {
        //     response = new CreateQuestionResponse
        //     {
        //         Status = CreateQuestionResponseStatus.InvalidPreviewId
        //     };
        //
        //     return new JsonResult(response);
        // }
        
        var category = _db.QuestionTypes.FirstOrDefault(e => e.Id == request.TypeId);
        if (category is null)
        {
            response = new CreateQuestionResponse
            {
                Status = CreateQuestionResponseStatus.InvalidTypeId
            };

            return new JsonResult(response);
        }

        var questionModel = _mapper.Map<QuestionModel>(request);
        _db.Questions.Add(questionModel);
        await _db.SaveChangesAsync();

        response = new CreateQuestionResponse
        {
            Status = CreateQuestionResponseStatus.Ok,
            Id = questionModel.Id
        };

        return new JsonResult(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<UpdateQuestionResponse>> Update(
        [FromBody] UpdateQuestionRequest request, 
        [FromHeader] string secretKey)
    {
        UpdateQuestionResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new UpdateQuestionResponse
            {
                Status = UpdateQuestionResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.FirstOrDefault(e => e.Id == request.Id);
        if (question is null)
        {
            response = new UpdateQuestionResponse
            {
                Status = UpdateQuestionResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == request.QuestId);
        if (quest is null)
        {
            response = new UpdateQuestionResponse
            {
                Status = UpdateQuestionResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        
        // var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        // if (preview is null)
        // {
        //     response = new UpdateQuestionResponse
        //     {
        //         Status = UpdateQuestionResponseStatus.InvalidPreviewId
        //     };
        //
        //     return new JsonResult(response);
        // }
        
        var type = _db.QuestionTypes.FirstOrDefault(e => e.Id == request.TypeId);
        if (type is null)
        {
            response = new UpdateQuestionResponse
            {
                Status = UpdateQuestionResponseStatus.InvalidTypeId
            };

            return new JsonResult(response);
        }

        _mapper.Map(request, question);
        _db.Questions.Update(question);
        await _db.SaveChangesAsync();

        response = new UpdateQuestionResponse
        {
            Status = UpdateQuestionResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpDelete]
    public async Task<ActionResult<DeleteQuestionResponse>> Delete(
        [FromQuery] int id, 
        [FromHeader] string secretKey)
    {
        DeleteQuestionResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteQuestionResponse
            {
                Status = DeleteQuestionResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.FirstOrDefault(e => e.Id == id);
        if (question is null)
        {
            response = new DeleteQuestionResponse
            {
                Status = DeleteQuestionResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }
        _db.Questions.Remove(question);
        await _db.SaveChangesAsync();

        response = new DeleteQuestionResponse
        {
            Status = DeleteQuestionResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetByIdQuestionResponse>> GetById(
        [FromHeader] int id, 
        [FromHeader] string token)
    {
        GetByIdQuestionResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetByIdQuestionResponse
            {
                Status = GetByIdQuestionResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetByIdQuestionResponse
            {
                Status = GetByIdQuestionResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetByIdQuestionResponse
            {
                Status = GetByIdQuestionResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }
        
        var question = _db.Questions.FirstOrDefault(e => e.Id == id);
        if (question is null)
        {
            response = new GetByIdQuestionResponse
            {
                Status = GetByIdQuestionResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }
        

        response = new GetByIdQuestionResponse
        {
            Status = GetByIdQuestionResponseStatus.Ok,
            Question = _mapper.Map<QuestionDto>(question)
        };

        return new JsonResult(response);
    }
   
    [HttpGet]
    public async Task<ActionResult<GetAllQuestionResponse>> GetAll([FromHeader] string token)
    {
        GetAllQuestionResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }

        var questionDtos = _db.Questions.ToArray().Select(e => _mapper.Map<QuestionDto>(e));


        response = new GetAllQuestionResponse
        {
            Status = GetAllQuestionResponseStatus.Ok,
            Questions = questionDtos
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetAllQuestionResponse>> GetAllByQuestId([FromHeader] string token, [FromQuery] int questId)
    {
        GetAllQuestionResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }
        
        var quest = _db.Quests.FirstOrDefault(e => e.Id == questId);
        if (quest is null)
        {
            response = new GetAllQuestionResponse
            {
                Status = GetAllQuestionResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        
        var questionDtos = _db.Questions
            .Where(e => e.QuestId == questId)
            .Select(e => _mapper.Map<QuestionDto>(e));


        response = new GetAllQuestionResponse
        {
            Status = GetAllQuestionResponseStatus.Ok,
            Questions = questionDtos
        };

        return new JsonResult(response);
    }
}