namespace KidQquest.Params.AuthParams;

public enum ConfirmResetPasswordResponseStatus
{
    Ok,
    UserNotExists,
    UserNotActive,
    InvalidCode, 
    WrongEmail
}