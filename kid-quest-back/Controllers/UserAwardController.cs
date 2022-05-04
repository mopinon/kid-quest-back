using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.UserAwardParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserAwardController : Controller
{
    private readonly ILogger<UserAwardController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public UserAwardController(
        ILogger<UserAwardController> logger,
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
    public async Task<ActionResult<CreateUserAwardResponse>> Create(
        [FromBody] CreateUserAwardRequest request,
        [FromHeader] string token)
    {
        CreateUserAwardResponse response;
        
        if (!_jwtService.IsJwtToken(token))
        {
            response = new CreateUserAwardResponse
            {
                Status = CreateUserAwardResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new CreateUserAwardResponse
            {
                Status = CreateUserAwardResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new CreateUserAwardResponse
            {
                Status = CreateUserAwardResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var userAwardOld = _db.UserAwards
            .FirstOrDefault(e => e.UserId == user.Id && e.AwardTypeId == request.AwardTypeId);

        if (userAwardOld is not null)
        {
            response = new CreateUserAwardResponse
            {
                Status = CreateUserAwardResponseStatus.UserAwardAlreadyExists,
                Id = userAwardOld.Id
            };

            return new JsonResult(response);
        }

        var userAwardModel = _mapper.Map<UserAwardModel>(request);
        userAwardModel.User = user;
        _db.UserAwards.Add(userAwardModel);
        await _db.SaveChangesAsync();

        response = new CreateUserAwardResponse
        {
            Status = CreateUserAwardResponseStatus.Ok,
            Id = userAwardModel.Id
        };

        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetAllUserAwardsResponse>> GetAllByUserToken([FromHeader] string token)
    {
        GetAllUserAwardsResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllUserAwardsResponse
            {
                Status = GetAllUserAwardsResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllUserAwardsResponse
            {
                Status = GetAllUserAwardsResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllUserAwardsResponse
            {
                Status = GetAllUserAwardsResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var userAwardDtos = _db.UserAwards
            .Include(e => e.AwardType)
            .Where(e => e.UserId == user.Id)
            .Select(e => _mapper.Map<AwardTypeDto>(e.AwardType));


        response = new GetAllUserAwardsResponse
        {
            Status = GetAllUserAwardsResponseStatus.Ok,
            UserAwards = userAwardDtos
        };

        return new JsonResult(response);
    }
}