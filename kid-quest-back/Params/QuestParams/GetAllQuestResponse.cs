using System.Collections.Generic;

namespace KidQquest.Params.QuestParams;

public class GetAllQuestResponse
{
    public GetAllQuestResponseStatus Status { get; set; }
    public IEnumerable<QuestDto> Quests { get; set; }
}