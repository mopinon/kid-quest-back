using System.Collections.Generic;

namespace KidQquest.Models;

public class QuestionModel
{
    public int Id { get; set; }
    public QuestModel Quest { get; set; }
    public int QuestId { get; set; }
    public string Text { get; set; }
    public PreviewModel? Preview { get; set; }
    public int? PreviewId { get; set; }
    public int Number { get; set; }
    public QuestionTypeModel Type { get; set; }
    public int TypeId { get; set; }
    public FactModel? Fact { get; set; }
    public ICollection<AnswerVariantModel> AnswerVariants { get; set; }
}