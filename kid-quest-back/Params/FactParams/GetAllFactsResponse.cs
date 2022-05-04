using System.Collections.Generic;

namespace KidQquest.Params.FactParams;

public class GetAllFactsResponse
{
    public GetAllFactsResponseStatus Status { get; set; }
    public IEnumerable<FactDto> Facts { get; set; }
}