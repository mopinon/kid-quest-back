using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.AttemptParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AttemptController : Controller
{
    private readonly ILogger<AttemptController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AttemptController(
        ILogger<AttemptController> logger,
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
    public async Task<ActionResult<CreateAttemptResponse>> Create(
        [FromBody] CreateAttemptRequest request,
        [FromHeader] string token)
    {
        CreateAttemptResponse response;
        
        if (!_jwtService.IsJwtToken(token))
        {
            response = new CreateAttemptResponse
            {
                Status = CreateAttemptResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new CreateAttemptResponse
            {
                Status = CreateAttemptResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new CreateAttemptResponse
            {
                Status = CreateAttemptResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == request.QuestId);
        if (quest is null)
        {
            response = new CreateAttemptResponse
            {
                Status = CreateAttemptResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        
        var attemptOld = _db.Attempts
            .FirstOrDefault(e => e.UserId == user.Id && e.QuestId == request.QuestId);

        if (attemptOld is not null)
        {
            response = new CreateAttemptResponse
            {
                Status = CreateAttemptResponseStatus.AttemptAlreadyExists,
                Id = attemptOld.Id
            };

            return new JsonResult(response);
        }

        var attemptModel = _mapper.Map<AttemptModel>(request);
        attemptModel.User = user;
        _db.Attempts.Add(attemptModel);
        await _db.SaveChangesAsync();

        response = new CreateAttemptResponse
        {
            Status = CreateAttemptResponseStatus.Ok,
            Id = attemptModel.Id
        };

        return new JsonResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<AttemptIsExistsResponse>> IsExists(
        [FromQuery] int questId,
        [FromHeader] string token)
    {
        AttemptIsExistsResponse response;
        
        if (!_jwtService.IsJwtToken(token))
        {
            response = new AttemptIsExistsResponse
            {
                Status = AttemptIsExistsResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new AttemptIsExistsResponse
            {
                Status = AttemptIsExistsResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new AttemptIsExistsResponse
            {
                Status = AttemptIsExistsResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var quest = _db.Quests.FirstOrDefault(e => e.Id == questId);
        if (quest is null)
        {
            response = new AttemptIsExistsResponse
            {
                Status = AttemptIsExistsResponseStatus.InvalidQuestId
            };

            return new JsonResult(response);
        }
        
        var attempt = _db.Attempts.FirstOrDefault(e => e.UserId == user.Id && e.QuestId == questId);

        if (attempt is null)
        {
            response = new AttemptIsExistsResponse
            {
                Status = AttemptIsExistsResponseStatus.IsNotExists
            };

            return new JsonResult(response);
        }

        response = new AttemptIsExistsResponse
        {
            Status = AttemptIsExistsResponseStatus.IsExists
        };

        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetAllAttemptsResponse>> GetAllByUserToken([FromHeader] string token)
    {
        GetAllAttemptsResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllAttemptsResponse
            {
                Status = GetAllAttemptsResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllAttemptsResponse
            {
                Status = GetAllAttemptsResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllAttemptsResponse
            {
                Status = GetAllAttemptsResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var attemptDtos = _db.Attempts
            .Where(e => e.UserId == user.Id)
            .Select(e => _mapper.Map<AttemptDto>(e));


        response = new GetAllAttemptsResponse
        {
            Status = GetAllAttemptsResponseStatus.Ok,
            Attempts = attemptDtos
        };

        return new JsonResult(response);
    }
}