namespace KidQquest.Models;

public class UserAwardModel
{
    public int Id { get; set; }
    public UserModel User { get; set; }
    public int UserId { get; set; }
    public AwardTypeModel AwardType { get; set; }
    public int AwardTypeId { get; set; }
}