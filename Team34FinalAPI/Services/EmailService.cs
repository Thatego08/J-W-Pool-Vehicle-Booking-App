using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Team34FinalAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendVerificationEmailAsync(string toEmail, string subject, string message);
        Task SendPasswordResetEmailAsync(string toEmail, string otpCode);
    }

    public class MailJetService : IEmailService
    {
        private readonly MailJetOptions _options;
        private readonly ILogger<MailJetService> _logger;

        public MailJetService(IOptions<MailJetOptions> options, ILogger<MailJetService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                // Validate configuration
                if (string.IsNullOrEmpty(_options.ApiKey) || string.IsNullOrEmpty(_options.SecretKey))
                {
                    throw new ArgumentException("MailJet API credentials are not configured properly.");
                }

                // Create Mailjet client
                var client = new MailjetClient(_options.ApiKey, _options.SecretKey);

                // Create the email request
                var request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.FromEmail, _options.SenderEmail)
                .Property(Send.FromName, _options.SenderName)
                .Property(Send.Subject, subject)
                .Property(Send.HtmlPart, message)
                .Property(Send.Recipients, new JArray
                {
                    new JObject
                    {
                        {"Email", toEmail}
                    }
                });

                // Send the email
                var response = await client.PostAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Email sent successfully to {toEmail}");
                }
                else
                {
                    var errorMessage = response.GetErrorMessage();
                    _logger.LogError($"Failed to send email. Status: {response.StatusCode}, Message: {errorMessage}");
                    throw new Exception($"Failed to send email: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email via MailJet");
                throw new Exception($"Email service error: {ex.Message}");
            }
        }

        // Add these methods to match the interface
        public async Task SendVerificationEmailAsync(string toEmail, string subject, string message)
        {
            // For now, just call the main SendEmailAsync method
            await SendEmailAsync(toEmail, subject, message);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string otpCode)
        {
            var subject = "Password Reset Request - JAWS System";
            var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: #dc3545; color: white; padding: 20px; text-align: center; }}
                        .content {{ background: #f9f9f9; padding: 20px; }}
                        .otp-code {{ font-size: 32px; font-weight: bold; text-align: center; color: #dc3545; margin: 20px 0; }}
                        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Password Reset</h1>
                        </div>
                        <div class='content'>
                            <p>We received a request to reset your password for your JAWS account.</p>
                            <p>Please use the following OTP code to reset your password:</p>
                            <div class='otp-code'>{otpCode}</div>
                            <p>This code will expire in 10 minutes.</p>
                            <p>If you didn't request a password reset, please ignore this email.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} JAWS System. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(toEmail, subject, htmlContent);
        }
    }

    public class MailJetOptions
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}