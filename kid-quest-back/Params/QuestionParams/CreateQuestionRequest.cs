namespace KidQquest.Params.QuestionParams;

public class CreateQuestionRequest
{
    public int QuestId { get; set; }
    public string Text { get; set; }
    public int? PreviewId { get; set; }
    public int Number { get; set; }
    public int TypeId { get; set; }
}