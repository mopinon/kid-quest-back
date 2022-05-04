using System.Collections.Generic;

namespace KidQquest.Params.QuestionParams;

public class GetAllQuestionResponse
{
    public GetAllQuestionResponseStatus Status { get; set; }
    public IEnumerable<QuestionDto> Questions { get; set; }
}