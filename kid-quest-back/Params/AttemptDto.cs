namespace KidQquest.Params;

public class AttemptDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuestId { get; set; }
    public int CorrectAnswersCount { get; set; }
}