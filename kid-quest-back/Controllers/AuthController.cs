using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params.AuthParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

public class AuthController : Controller
{
    private readonly ApplicationContext _db;
    private readonly IEmailService _emailService;
    private readonly IHashService _hashService;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ILogger<AuthController> logger,
        ApplicationContext context,
        IEmailService emailService,
        IJwtService jwtService,
        IHashService hashService,
        IMapper mapper)
    {
        _logger = logger;
        _db = context;
        _emailService = emailService;
        _jwtService = jwtService;
        _hashService = hashService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        RegisterResponse response;

        var email = request.Email.ToLower();
        var user = _db.Users.FirstOrDefault(u => u.Email == email);

        if (user is { Status: not UserStatus.Unconfirmed })
        {
            response = new RegisterResponse
            {
                Status = RegisterResponseStatus.UserExists
            };
            return new JsonResult(response);
        }

        var passwordHash = _hashService.HashPassword(request.Password);
        var code = StringUtil.RandomString(6);
        var sendEmailResult = await _emailService.SendEmailAsync(email, "Код подтверждения регистрации", code);
        if (sendEmailResult == SendEmailResult.InvalidEmail)
        {
            response = new RegisterResponse
            {
                Status = RegisterResponseStatus.InvalidEmail
            };
            return new JsonResult(response);
        }

        if (user is { Status: UserStatus.Unconfirmed })
        {
            user.PasswordHash = passwordHash;
            user.EmailConfirmationCode = code;
            user.Name = request.Name;

            _db.Update(user);
        }
        else
        {
            user ??= new UserModel();
            user.Email = email;
            user.PasswordHash = passwordHash;
            user.Status = UserStatus.Unconfirmed;
            user.EmailConfirmationCode = code;
            user.Name = request.Name;

            _db.Users.Add(user);
        }

        await _db.SaveChangesAsync();

        response = new RegisterResponse
        {
            Status = RegisterResponseStatus.Ok
        };
        return new JsonResult(response);
    }

    [HttpPost("confirmEmail")]
    public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        ConfirmEmailResponse response;

        var email = request.Email.ToLower();
        var code = request.Code;
        var user = _db.Users.FirstOrDefault(u => u.Email == email);

        if (user is null)
        {
            response = new ConfirmEmailResponse
            {
                Status = ConfirmEmailResponseStatus.UserNotExists
            };

            return new JsonResult(response);
        }

        if (user.EmailConfirmationCode != code)
        {
            response = new ConfirmEmailResponse
            {
                Status = ConfirmEmailResponseStatus.InvalidCode
            };

            return new JsonResult(response);
        }

        if (user.Status == UserStatus.Active)
        {
            response = new ConfirmEmailResponse
            {
                Status = ConfirmEmailResponseStatus.UserAlreadyActive
            };

            return new JsonResult(response);
        }

        user.Status = UserStatus.Active;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        response = new ConfirmEmailResponse
        {
            Status = ConfirmEmailResponseStatus.Ok
        };

        return new JsonResult(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        LoginResponse response;

        var email = request.Email.ToLower();
        var password = request.Password;
        var user = _db.Users.FirstOrDefault(e => e.Email == email);

        if (user is null)
        {
            response = new LoginResponse
            {
                Status = LoginResponseStatus.UserNotExists
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new LoginResponse
            {
                Status = LoginResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var isValidPassword = _hashService.VerifyHashedPassword(user.PasswordHash, password);
        if (!isValidPassword)
        {
            response = new LoginResponse
            {
                Status = LoginResponseStatus.InvalidPassword
            };

            return new JsonResult(response);
        }

        var token = _jwtService.Encode(email);
        response = new LoginResponse
        {
            Status = LoginResponseStatus.Ok,
            Token = token
        };

        return new JsonResult(response);
    }

    [HttpGet("loginWithToken")]
    public async Task<ActionResult<LoginWithTokenResponse>> LoginWithToken([FromHeader] string token)
    {
        LoginWithTokenResponse response;

        var isJwtToken = _jwtService.IsJwtToken(token);
        if (!isJwtToken)
        {
            response = new LoginWithTokenResponse
            {
                Status = LoginWithTokenResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token);
        var user = _db.Users.FirstOrDefault(e => e.Email == email);

        if (user is null)
        {
            response = new LoginWithTokenResponse
            {
                Status = LoginWithTokenResponseStatus.UserNotExists
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new LoginWithTokenResponse
            {
                Status = LoginWithTokenResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        response = new LoginWithTokenResponse
        {
            Status = LoginWithTokenResponseStatus.Ok
        };

        return new JsonResult(response);
    }

    [HttpPost("resetPassword")]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        ResetPasswordResponse response;

        var email = request.Email.ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);

        if (user is null)
        {
            response = new ResetPasswordResponse
            {
                Status = ResetPasswordResponseStatus.UserNotExists
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new ResetPasswordResponse
            {
                Status = ResetPasswordResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var resetPasswordModelOld = _db.ResetPasswords
            .Include(e => e.User)
            .FirstOrDefault(e => e.User == user);

        var resetPasswordCode = StringUtil.RandomString(6);
        var resetPasswordModel = new ResetPasswordModel
        {
            Code = resetPasswordCode,
            User = user
        };
        
        if (resetPasswordModelOld is null)
        {
            _db.ResetPasswords.Add(resetPasswordModel);
        }
        else
        {
            resetPasswordModelOld.Code = resetPasswordCode;
            _db.ResetPasswords.Update(resetPasswordModel);
        }
        
        await _db.SaveChangesAsync();

        await _emailService.SendEmailAsync(email, "Код подтверждения сброса пароля", resetPasswordCode);

        response = new ResetPasswordResponse
        {
            Status = ResetPasswordResponseStatus.Ok
        };

        return new JsonResult(response);
    }
    
    [HttpPost("confirmResetPassword")]
    public async Task<ActionResult<ConfirmResetPasswordResponse>> ConfirmResetPassword([FromBody] ConfirmResetPasswordRequest request)
    {
        ConfirmResetPasswordResponse response;

        var email = request.Email.ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);

        if (user is null)
        {
            response = new ConfirmResetPasswordResponse
            {
                Status = ConfirmResetPasswordResponseStatus.UserNotExists
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new ConfirmResetPasswordResponse
            {
                Status = ConfirmResetPasswordResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var resetPasswordModel = _db.ResetPasswords
            .Include(e => e.User)
            .FirstOrDefault(e => e.Code == request.Code.ToUpper());

        if (resetPasswordModel is null)
        {
            response = new ConfirmResetPasswordResponse
            {
                Status = ConfirmResetPasswordResponseStatus.InvalidCode
            };

            return new JsonResult(response);
        }

        if (resetPasswordModel.User.Email != email)
        {
            response = new ConfirmResetPasswordResponse
            {
                Status = ConfirmResetPasswordResponseStatus.WrongEmail
            };

            return new JsonResult(response);
        }

        user.PasswordHash = _hashService.HashPassword(request.NewPassword);
        _db.ResetPasswords.Remove(resetPasswordModel);
        _db.Users.Update(user);

        await _db.SaveChangesAsync();

        response = new ConfirmResetPasswordResponse
        {
            Status = ConfirmResetPasswordResponseStatus.Ok
        };
        
        return new JsonResult(response);
    }
}