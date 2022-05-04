namespace KidQquest.Models;

public class AnswerVariantModel
{
    public int Id { get; set; }
    public QuestionModel Question { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public PreviewModel? Preview { get; set; }
    public int? PreviewId { get; set; }
    public bool IsRight { get; set; }
}