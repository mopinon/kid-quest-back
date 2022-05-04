using System.Collections.Generic;

namespace KidQquest.Params.QuestionTypeParams;

public class GetAllQuestionTypeResponse
{
    public GetAllQuestionTypeResponseStatus Status { get; set; }
    public IEnumerable<QuestionTypeDto> QuestionTypes { get; set; }
}