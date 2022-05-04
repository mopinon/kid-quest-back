using System.Collections.Generic;
using System.Linq;

namespace KidQquest.Params.UserAwardParams;

public class GetAllUserAwardsResponse
{
    public GetAllUserAwardsResponseStatus Status { get; set; }
    public IEnumerable<AwardTypeDto> UserAwards { get; set; }
}