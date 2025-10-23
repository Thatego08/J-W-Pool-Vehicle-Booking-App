//using Microsoft.Extensions.Options;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System.Threading.Tasks;

//namespace Team34FinalAPI.Services
//{
//    public interface IEmailService
//    {
//        Task SendEmailAsync(string toEmail, string subject, string message);
//    }

//    public class TwilioEmailService : IEmailService
//    {
//        private readonly TwilioSettings _twilioSettings;
//        private readonly ILogger<TwilioEmailService> _logger;

//        public TwilioEmailService(IOptions<TwilioSettings> twilioSettings, ILogger<TwilioEmailService> logger)
//        {
//            _twilioSettings = twilioSettings.Value;
//            _logger = logger;
//        }

//        public async Task SendEmailAsync(string toEmail, string subject, string message)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(_twilioSettings.SendGridApiKey))
//                {
//                    throw new ArgumentException("SendGrid API Key is not configured");
//                }

//                var client = new SendGridClient(_twilioSettings.SendGridApiKey);
//                var from = new EmailAddress(_twilioSettings.FromEmail, _twilioSettings.FromName);
//                var to = new EmailAddress(toEmail);
//                var plainTextContent = message;
//                var htmlContent = $"<p>{message}</p>";
//                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

//                var response = await client.SendEmailAsync(msg);

//                if (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
//                    response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    _logger.LogInformation($"Email sent successfully to {toEmail}");
//                }
//                else
//                {
//                    var responseBody = await response.Body.ReadAsStringAsync();
//                    _logger.LogError($"Failed to send email. Status: {response.StatusCode}, Body: {responseBody}");
//                    throw new Exception($"Failed to send email: {response.StatusCode}");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error sending email via Twilio SendGrid");
//                throw new Exception($"Email service error: {ex.Message}");
//            }
//        }
//    }

//    public class TwilioSettings
//    {
//        public string SendGridApiKey { get; set; }
//        public string FromEmail { get; set; }
//        public string FromName { get; set; }
//    }
//}