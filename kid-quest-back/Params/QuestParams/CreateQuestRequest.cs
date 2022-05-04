namespace KidQquest.Params.QuestParams;

public class CreateQuestRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreviewId { get; set; }
    public int CategoryId { get; set; }
}