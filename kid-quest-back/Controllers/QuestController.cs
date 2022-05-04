using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.QuestParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class QuestController : Controller
{
    private readonly ILogger<QuestController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    
    public QuestController(
        ILogger<QuestController> logger,
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
    public async Task<ActionResult<CreateQuestResponse>> Create(
        [FromBody] CreateQuestRequest request, 
        [FromHeader] string secretKey)
    {
        CreateQuestResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateQuestResponse
            {
                Status = CreateQuestResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        if (preview is null)
        {
            response = new CreateQuestResponse
            {
                Status = CreateQuestResponseStatus.InvalidPreviewId
            };

            return new JsonResult(response);
        }
        
        var category = _db.QuestCategories.FirstOrDefault(e => e.Id == request.CategoryId);
        if (category is null)
        {
            response = new CreateQuestResponse
            {
                Status = CreateQuestResponseStatus.InvalidCategoryId
            };

            return new JsonResult(response);
        }

        var questModel = _mapper.Map<QuestModel>(request);
        _db.Quests.Add(questModel);
        await _db.SaveChangesAsync();

        response = new CreateQuestResponse
        {
            Status = CreateQuestResponseStatus.Ok,
            Id = questModel.Id
        };

        return new JsonResult(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<UpdateQuestResponse>> Update(
        [FromBody] UpdateQuestRequest request, 
        [FromHeader] string secretKey)
    {
        UpdateQuestResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new UpdateQuestResponse
            {
                Status = UpdateQuestResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == request.Id);
        if (quest is null)
        {
            response = new UpdateQuestResponse
            {
                Status = UpdateQuestResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }

        var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        if (preview is null)
        {
            response = new UpdateQuestResponse
            {
                Status = UpdateQuestResponseStatus.InvalidPreviewId
            };

            return new JsonResult(response);
        }
        
        var category = _db.QuestCategories.FirstOrDefault(e => e.Id == request.CategoryId);
        if (category is null)
        {
            response = new UpdateQuestResponse
            {
                Status = UpdateQuestResponseStatus.InvalidCategoryId
            };

            return new JsonResult(response);
        }

        _mapper.Map(request, quest);
        _db.Quests.Update(quest);
        await _db.SaveChangesAsync();

        response = new UpdateQuestResponse
        {
            Status = UpdateQuestResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpDelete]
    public async Task<ActionResult<DeleteQuestResponse>> Delete(
        [FromQuery] int id, 
        [FromHeader] string secretKey)
    {
        DeleteQuestResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteQuestResponse
            {
                Status = DeleteQuestResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == id);
        if (quest is null)
        {
            response = new DeleteQuestResponse
            {
                Status = DeleteQuestResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        _db.Quests.Remove(quest);
        await _db.SaveChangesAsync();

        response = new DeleteQuestResponse
        {
            Status = DeleteQuestResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetByIdQuestResponse>> GetById(
        [FromHeader] int id, 
        [FromHeader] string token)
    {
        GetByIdQuestResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetByIdQuestResponse
            {
                Status = GetByIdQuestResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetByIdQuestResponse
            {
                Status = GetByIdQuestResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetByIdQuestResponse
            {
                Status = GetByIdQuestResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }
        
        var quest = _db.Quests.Include(e => e.Questions).FirstOrDefault(e => e.Id == id);
        if (quest is null)
        {
            response = new GetByIdQuestResponse
            {
                Status = GetByIdQuestResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        

        response = new GetByIdQuestResponse
        {
            Status = GetByIdQuestResponseStatus.Ok,
            Quest = _mapper.Map<QuestDto>(quest)
        };
        response.Quest.QuestionsCount = quest.Questions.Count;

        return new JsonResult(response);
    }
   
    [HttpGet]
    public async Task<ActionResult<GetAllQuestResponse>> GetAll([FromHeader] string token)
    {
        GetAllQuestResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllQuestResponse
            {
                Status = GetAllQuestResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllQuestResponse
            {
                Status = GetAllQuestResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllQuestResponse
            {
                Status = GetAllQuestResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }

        var questDtos = _db.Quests.Include(e => e.Questions).ToArray().Select(e =>
        {
            var questDto = _mapper.Map<QuestDto>(e);
            questDto.QuestionsCount = e.Questions.Count;
            return questDto;
        });


        response = new GetAllQuestResponse
        {
            Status = GetAllQuestResponseStatus.Ok,
            Quests = questDtos
        };

        return new JsonResult(response);
    }
}