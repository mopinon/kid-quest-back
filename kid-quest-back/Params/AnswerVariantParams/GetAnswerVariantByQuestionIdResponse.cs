using System.Collections.Generic;

namespace KidQquest.Params.AnswerVariantParams;

public class GetAnswerVariantByQuestionIdResponse
{
    public GetAnswerVariantByQuestionIdResponseStatus Status { get; set; }
    public IEnumerable<AnswerVariantDto> AnswerVariants { get; set; }
}