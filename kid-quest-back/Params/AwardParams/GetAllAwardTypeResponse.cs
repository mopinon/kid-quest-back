using System.Collections.Generic;

namespace KidQquest.Params.AwardParams;

public class GetAllAwardTypeResponse
{
    public GetAllAwardTypeResponseStatus Status { get; set; }
    public IEnumerable<AwardTypeDto> AwardTypes { get; set; }
}