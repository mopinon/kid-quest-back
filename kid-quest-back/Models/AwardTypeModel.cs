namespace KidQquest.Models;

public class AwardTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PreviewModel Preview { get; set; }
    public int PreviewId { get; set; }
    public QuestModel Quest { get; set; }
    public int QuestId { get; set; }
}