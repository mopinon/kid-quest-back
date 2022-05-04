using System.Collections.Generic;

namespace KidQquest.Params;

public class QuestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreviewId { get; set; }
    public int CategoryId { get; set; }
    public int QuestionsCount { get; set; }
}