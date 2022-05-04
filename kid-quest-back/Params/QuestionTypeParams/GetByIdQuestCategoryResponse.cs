namespace KidQquest.Params.QuestionTypeParams;

public class GetByIdQuestionTypeResponse
{
    public GetByIdQuestionTypeResponseStatus Status { get; set; }
    public QuestionTypeDto QuestionType { get; set; }
}