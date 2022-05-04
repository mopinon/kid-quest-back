namespace KidQquest.Params.QuestionParams;

public class GetByIdQuestionResponse
{
    public GetByIdQuestionResponseStatus Status { get; set; }
    public QuestionDto Question { get; set; }
}