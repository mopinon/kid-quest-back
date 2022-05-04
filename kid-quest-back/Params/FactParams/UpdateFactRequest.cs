namespace KidQquest.Params.FactParams;

public class UpdateFactRequest
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public int? PreviewId { get; set; }
}