using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

using practice.Models;

namespace practice.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Voting System", _emailSettings.FromEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                // This is the specific line that fixes Gmail connection issues
                await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);

                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // This helps us see the REAL error in the browser if it fails again
                throw new InvalidOperationException($"MailKit Error: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}