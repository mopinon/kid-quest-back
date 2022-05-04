namespace KidQquest.Params.AnswerVariantParams;

public class UpdateAnswerVariantRequest
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public int? PreviewId { get; set; }
    public bool IsRight { get; set; }
}