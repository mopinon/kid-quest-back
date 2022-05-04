using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.AnswerVariantParams;
using KidQquest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KidQquest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AnswerVariantController : Controller
{
    private readonly ILogger<AnswerVariantController> _logger;
    private readonly ApplicationContext _db;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AnswerVariantController(
        ILogger<AnswerVariantController> logger,
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
    public async Task<ActionResult<CreateAnswerVariantResponse>> Create(
        [FromBody] CreateAnswerVariantRequest request,
        [FromHeader] string secretKey)
    {
        CreateAnswerVariantResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new CreateAnswerVariantResponse
            {
                Status = CreateAnswerVariantResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.FirstOrDefault(e => e.Id == request.QuestionId);
        if (question is null)
        {
            response = new CreateAnswerVariantResponse
            {
                Status = CreateAnswerVariantResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        // var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        // if (preview is null)
        // {
        //     response = new CreateAnswerVariantResponse
        //     {
        //         Status = CreateAnswerVariantResponseStatus.InvalidPreviewId
        //     };
        //
        //     return new JsonResult(response);
        // }

        var answerVariantModel = _mapper.Map<AnswerVariantModel>(request);
        _db.AnswerVariants.Add(answerVariantModel);
        await _db.SaveChangesAsync();

        response = new CreateAnswerVariantResponse
        {
            Status = CreateAnswerVariantResponseStatus.Ok,
            Id = answerVariantModel.Id
        };

        return new JsonResult(response);
    }

    [HttpPost]
    public async Task<ActionResult<UpdateAnswerVariantResponse>> Update(
        [FromBody] UpdateAnswerVariantRequest request,
        [FromHeader] string secretKey)
    {
        UpdateAnswerVariantResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new UpdateAnswerVariantResponse
            {
                Status = UpdateAnswerVariantResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var answerVariant = _db.AnswerVariants.FirstOrDefault(e => e.Id == request.Id);
        if (answerVariant is null)
        {
            response = new UpdateAnswerVariantResponse
            {
                Status = UpdateAnswerVariantResponseStatus.InvalidAnswerVariantId
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.FirstOrDefault(e => e.Id == request.QuestionId);
        if (question is null)
        {
            response = new UpdateAnswerVariantResponse
            {
                Status = UpdateAnswerVariantResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        // var preview = _db.Previews.FirstOrDefault(e => e.Id == request.PreviewId);
        // if (preview is null)
        // {
        //     response = new UpdateAnswerVariantResponse
        //     {
        //         Status = UpdateAnswerVariantResponseStatus.InvalidPreviewId
        //     };
        //
        //     return new JsonResult(response);
        // }

        _mapper.Map(request, answerVariant);
        _db.AnswerVariants.Update(answerVariant);
        await _db.SaveChangesAsync();

        response = new UpdateAnswerVariantResponse
        {
            Status = UpdateAnswerVariantResponseStatus.Ok
        };

        return new JsonResult(response);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteAnswerVariantResponse>> Delete(
        [FromQuery] int id,
        [FromHeader] string secretKey)
    {
        DeleteAnswerVariantResponse response;
        if (secretKey != SecretKeyProvider.SecretKey)
        {
            response = new DeleteAnswerVariantResponse
            {
                Status = DeleteAnswerVariantResponseStatus.InvalidSecretKey
            };

            return new JsonResult(response);
        }

        var answerVariant = _db.AnswerVariants.FirstOrDefault(e => e.Id == id);
        if (answerVariant is null)
        {
            response = new DeleteAnswerVariantResponse
            {
                Status = DeleteAnswerVariantResponseStatus.InvalidAnswerVariantId
            };

            return new JsonResult(response);
        }

        _db.AnswerVariants.Remove(answerVariant);
        await _db.SaveChangesAsync();

        response = new DeleteAnswerVariantResponse
        {
            Status = DeleteAnswerVariantResponseStatus.Ok
        };

        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetAnswerVariantByQuestionIdResponse>> GetByQuestionId(
        [FromHeader] int questionId,
        [FromHeader] string token)
    {
        GetAnswerVariantByQuestionIdResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAnswerVariantByQuestionIdResponse
            {
                Status = GetAnswerVariantByQuestionIdResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAnswerVariantByQuestionIdResponse
            {
                Status = GetAnswerVariantByQuestionIdResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAnswerVariantByQuestionIdResponse
            {
                Status = GetAnswerVariantByQuestionIdResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var question = _db.Questions.Include(e => e.AnswerVariants).FirstOrDefault(e => e.Id == questionId);
        if (question is null)
        {
            response = new GetAnswerVariantByQuestionIdResponse
            {
                Status = GetAnswerVariantByQuestionIdResponseStatus.InvalidQuestionId
            };

            return new JsonResult(response);
        }

        var answerVariants = question.AnswerVariants.Select(e => _mapper.Map<AnswerVariantDto>(e));

        response = new GetAnswerVariantByQuestionIdResponse
        {
            Status = GetAnswerVariantByQuestionIdResponseStatus.Ok,
            AnswerVariants = answerVariants
        };

        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetAllAnswerVariantsResponse>> GetAll([FromHeader] string token)
    {
        GetAllAnswerVariantsResponse response;

        if (!_jwtService.IsJwtToken(token))
        {
            response = new GetAllAnswerVariantsResponse
            {
                Status = GetAllAnswerVariantsResponseStatus.InvalidToken
            };

            return new JsonResult(response);
        }

        var email = _jwtService.Decode(token).ToLower();
        var user = _db.Users.FirstOrDefault(e => e.Email == email);
        if (user is null)
        {
            response = new GetAllAnswerVariantsResponse
            {
                Status = GetAllAnswerVariantsResponseStatus.UserNotFound
            };

            return new JsonResult(response);
        }

        if (user.Status != UserStatus.Active)
        {
            response = new GetAllAnswerVariantsResponse
            {
                Status = GetAllAnswerVariantsResponseStatus.UserNotActive
            };

            return new JsonResult(response);
        }

        var answerVariantDtos = _db.AnswerVariants.ToArray().Select(e => _mapper.Map<AnswerVariantDto>(e));


        response = new GetAllAnswerVariantsResponse
        {
            Status = GetAllAnswerVariantsResponseStatus.Ok,
            AnswerVariants = answerVariantDtos
        };

        return new JsonResult(response);
    }
}