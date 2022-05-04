using System.Collections.Generic;

namespace KidQquest.Params.QuestCategoryParams;

public class GetAllQuestCategoryResponse
{
    public GetAllQuestCategoryResponseStatus Status { get; set; }
    public IEnumerable<QuestCategoryDto> QuestCategories { get; set; }
}