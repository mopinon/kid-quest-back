using System.Collections.Generic;

namespace KidQquest.Models;

public class QuestModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreviewId { get; set; }
    public PreviewModel Preview { get; set; }
    public QuestCategoryModel Category { get; set; }
    public int CategoryId { get; set; }
    public ICollection<QuestionModel> Questions { get; set; }
    public AwardTypeModel AwardType { get; set; }
}