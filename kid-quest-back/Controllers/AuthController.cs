using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params.AuthParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers
{
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
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
        {
            RegisterResponse response;

            var email = request.Email.ToLower();
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user is { Status: not UserStatus.Unconfirmed })
            {
                response = new RegisterResponse
                {
                    Status = RegisterResponseStatus.UserExist
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

                _db.Update(user);
            }
            else
            {
                user ??= new UserModel();
                user.Email = email;
                user.PasswordHash = passwordHash;
                user.Status = UserStatus.Unconfirmed;
                user.EmailConfirmationCode = code;
                
                _db.Users.Add(user);
            }

            await _db.SaveChangesAsync();

            response = new RegisterResponse
            {
                Status = RegisterResponseStatus.Ok
            };
            return new JsonResult(response);
        }
    }
}