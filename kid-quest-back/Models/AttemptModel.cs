namespace KidQquest.Models;

public class AttemptModel
{
    public int Id { get; set; }
    public UserModel User { get; set; }
    public int UserId { get; set; }
    public QuestModel Quest { get; set; }
    public int QuestId { get; set; }
    public int CorrectAnswersCount { get; set; }
}