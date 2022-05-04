namespace KidQquest.Params.FactParams;

public class CreateFactRequest
{
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public int? PreviewId { get; set; }
}