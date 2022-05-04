namespace KidQquest.Params.AttemptParams;

public enum CreateAttemptResponseStatus
{
    Ok,
    InvalidToken,
    UserNotFound,
    UserNotActive,
    AttemptAlreadyExists,
    InvalidQuestId
}