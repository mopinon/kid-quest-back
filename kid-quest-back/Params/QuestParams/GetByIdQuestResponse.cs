namespace KidQquest.Params.QuestParams;

public class GetByIdQuestResponse
{
    public GetByIdQuestResponseStatus Status { get; set; }
    public QuestDto Quest { get; set; }
}