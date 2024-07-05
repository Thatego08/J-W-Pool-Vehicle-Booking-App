using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            var settings = smtpSettings.Value;

            _smtpClient = new SmtpClient(settings.Host, settings.Port)
            {
                Credentials = new NetworkCredential(settings.Username, settings.Password),
                EnableSsl = settings.EnableSsl
            };
            _fromEmail = settings.FromEmail;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, message);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
