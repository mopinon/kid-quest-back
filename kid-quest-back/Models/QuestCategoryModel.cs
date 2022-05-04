using System.Collections.Generic;

namespace KidQquest.Models;

public class QuestCategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<QuestModel> Quests { get; set; }
}