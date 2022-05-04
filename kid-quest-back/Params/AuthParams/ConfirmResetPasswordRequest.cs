namespace KidQquest.Params.AuthParams;

public class ConfirmResetPasswordRequest
{
    public string Code { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}