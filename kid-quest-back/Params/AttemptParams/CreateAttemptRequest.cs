namespace KidQquest.Params.AttemptParams;

public class CreateAttemptRequest
{
    public int QuestId { get; set; }
    public int CorrectAnswersCount { get; set; }
}