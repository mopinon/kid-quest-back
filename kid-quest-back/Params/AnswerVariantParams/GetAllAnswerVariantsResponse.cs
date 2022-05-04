using System.Collections.Generic;

namespace KidQquest.Params.AnswerVariantParams;

public class GetAllAnswerVariantsResponse
{
    public GetAllAnswerVariantsResponseStatus Status { get; set; }
    public IEnumerable<AnswerVariantDto> AnswerVariants { get; set; }
}