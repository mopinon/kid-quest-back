using System.Collections.Generic;

namespace KidQquest.Params.AttemptParams;

public class GetAllAttemptsResponse
{
    public GetAllAttemptsResponseStatus Status { get; set; }
    public IEnumerable<AttemptDto> Attempts { get; set; }
}