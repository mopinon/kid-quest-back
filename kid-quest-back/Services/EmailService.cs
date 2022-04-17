using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace KidQquest.Services
{
    public class EmailService : IEmailService
    {
        public async Task<SendEmailResult> SendEmailAsync(string email, string subject, string message)
        {
            if (!email.IsValidEmail())
                return SendEmailResult.InvalidEmail;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("kidquest", "kidquest@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.mail.ru", 25, false);
            await client.AuthenticateAsync("kidquest@mail.ru", "Qqsv2fE00jfrnMDuHGbt");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);

            return SendEmailResult.Ok;
        }
    }
}