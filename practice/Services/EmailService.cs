using System.Net;
using System.Net.Mail;

namespace practice.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var mail = _configuration["EmailSettings:FromEmail"];
            var pw = _configuration["EmailSettings:Password"];

            var client = new SmtpClient("smtp.gmail.com", 587)

            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            var mailMessage = new MailMessage(from: mail, to: toEmail, subject, body);

            await client.SendMailAsync(mailMessage);
        }
    }
}