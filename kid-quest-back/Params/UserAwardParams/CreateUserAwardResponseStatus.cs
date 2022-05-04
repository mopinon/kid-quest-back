namespace KidQquest.Params.UserAwardParams;

public enum CreateUserAwardResponseStatus
{
    Ok,
    InvalidToken,
    UserNotFound,
    UserNotActive,
    UserAwardAlreadyExists
}