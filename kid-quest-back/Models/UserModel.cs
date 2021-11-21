using System.ComponentModel.DataAnnotations.Schema;
using KidQquest.Services;

namespace KidQquest.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string EmailConfirmationCode { get; set; }

        [NotMapped]
        public UserStatus Status { get; set; }
        
        [Column("Status")]
        public string UserStatusString
        {
            get => Status.ToString();
            set => Status = value.ParseEnum<UserStatus>();
        }
    }
}