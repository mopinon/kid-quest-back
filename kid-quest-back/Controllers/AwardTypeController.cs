using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.AwardParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AwardTypeController : Controller
{
    private readonly ILogger<AwardTypeController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    
    public AwardTypeController(
        ILogger<AwardTypeController> logger,
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
    public async Task<ActionResult<CreateAwardTypeResponse>> Create(
        [FromBody] CreateAwardTypeRequest request, 
        [FromHeader] string secretKey)
    {
        CreateAwardTypeResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateAwardTypeResponse
            {
                Status = CreateAwardTypeResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == request.QuestId);
        if (quest is null)
        {
            response = new CreateAwardTypeResponse
            {
                Status = CreateAwardTypeResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }

        var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        if (preview is null)
        {
            response = new CreateAwardTypeResponse
            {
                Status = CreateAwardTypeResponseStatus.InvalidPreviewId
            };

            return new JsonResult(response);
        }
        
        var awardTypeModel = _mapper.Map<AwardTypeModel>(request);
        _db.AwardTypes.Add(awardTypeModel);
        await _db.SaveChangesAsync();

        response = new CreateAwardTypeResponse
        {
            Status = CreateAwardTypeResponseStatus.Ok,
            Id = awardTypeModel.Id
        };

        return new JsonResult(response);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteAwardTypeResponse>> Delete(
        [FromQuery] int id, 
        [FromHeader] string secretKey)
    {
        DeleteAwardTypeResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteAwardTypeResponse
            {
                Status = DeleteAwardTypeResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var awardType = _db.AwardTypes.FirstOrDefault(e => e.Id == id);
        if (awardType is null)
        {
            response = new DeleteAwardTypeResponse
            {
                Status = DeleteAwardTypeResponseStatus.InvalidAwardTypeId
            };

            return new JsonResult(response);
        }
        _db.AwardTypes.Remove(awardType);
        await _db.SaveChangesAsync();

        response = new DeleteAwardTypeResponse
        {
            Status = DeleteAwardTypeResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetByIdAwardTypeResponse>> GetById(
        [FromHeader] int id, 
        [FromHeader] string token)
    {
        GetByIdAwardTypeResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetByIdAwardTypeResponse
            {
                Status = GetByIdAwardTypeResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetByIdAwardTypeResponse
            {
                Status = GetByIdAwardTypeResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetByIdAwardTypeResponse
            {
                Status = GetByIdAwardTypeResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }
        
        var awardType = _db.AwardTypes.FirstOrDefault(e => e.Id == id);
        if (awardType is null)
        {
            response = new GetByIdAwardTypeResponse
            {
                Status = GetByIdAwardTypeResponseStatus.InvalidAwardTypeId
            };

            return new JsonResult(response);
        }
        

        response = new GetByIdAwardTypeResponse
        {
            Status = GetByIdAwardTypeResponseStatus.Ok,
            AwardType = _mapper.Map<AwardTypeDto>(awardType)
        };

        return new JsonResult(response);
    }
   
    [HttpGet]
    public async Task<ActionResult<GetAllAwardTypeResponse>> GetAll([FromHeader] string token)
    {
        GetAllAwardTypeResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllAwardTypeResponse
            {
                Status = GetAllAwardTypeResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllAwardTypeResponse
            {
                Status = GetAllAwardTypeResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllAwardTypeResponse
            {
                Status = GetAllAwardTypeResponseStatus.UserNotActive
            };
            
            return new JsonResult(response);
        }

        var awardTypeDtos = _db.AwardTypes.ToArray().Select(e => _mapper.Map<AwardTypeDto>(e));
        
        response = new GetAllAwardTypeResponse
        {
            Status = GetAllAwardTypeResponseStatus.Ok,
            AwardTypes = awardTypeDtos
        };

        return new JsonResult(response);
    }
}