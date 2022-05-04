namespace KidQquest.Params.FactParams;

public enum GetFactByQuestionIdResponseStatus
{
    Ok,
    InvalidToken,
    UserNotFound,
    UserNotActive,
    InvalidQuestionId,
    FactIsNull
}