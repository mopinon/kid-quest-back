namespace KidQquest.Params.QuestCategoryParams;

public class GetByIdQuestCategoryResponse
{
    public GetByIdQuestCategoryResponseStatus Status { get; set; }
    public QuestCategoryDto QuestCategory { get; set; }
}