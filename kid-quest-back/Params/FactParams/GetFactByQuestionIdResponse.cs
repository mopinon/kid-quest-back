namespace KidQquest.Params.FactParams;

public class GetFactByQuestionIdResponse
{
    public GetFactByQuestionIdResponseStatus Status { get; set; }
    public FactDto Fact { get; set; }
}