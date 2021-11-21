using System.Threading.Tasks;

namespace KidQquest.Services
{
    public interface IEmailService
    {
        public Task<SendEmailResult> SendEmailAsync(string email, string subject, string message);

    }
}