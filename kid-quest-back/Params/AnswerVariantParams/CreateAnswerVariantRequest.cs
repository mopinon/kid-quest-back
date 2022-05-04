namespace KidQquest.Params.AnswerVariantParams;

public class CreateAnswerVariantRequest
{
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public int? PreviewId { get; set; }
    public bool IsRight { get; set; }
}