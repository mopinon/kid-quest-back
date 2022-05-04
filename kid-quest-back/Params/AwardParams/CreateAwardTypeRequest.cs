namespace KidQquest.Params.AwardParams;

public class CreateAwardTypeRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreviewId { get; set; }
    public int QuestId { get; set; }
}