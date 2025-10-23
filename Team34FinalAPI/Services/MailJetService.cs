//using Mailjet.Client;
//using Mailjet.Client.TransactionalEmails;
//using Mailjet.Client.Resources;
//using Newtonsoft.Json.Linq;
//using Microsoft.Extensions.Options;
//using Team34FinalAPI.Models;
//using Microsoft.AspNetCore.Identity;
//using User = Team34FinalAPI.Models.User;

//namespace Team34FinalAPI.Services
//{
//    public class MailJetService : IEmailService
//    {
//        private readonly MailJetOptions _options;

//        private readonly UserManager<User> _userManager;

//        public MailJetService(IOptions<MailJetOptions> options, UserManager<User> userManager)
//        {
//            _options = options.Value;

//            _userManager = userManager;
//        }

//        public async Task SendEmailAsync(string toEmail, string subject, string body)
//        {
//            var client = new MailjetClient(_options.ApiKey, _options.SecretKey);
//            var request = new MailjetRequest
//            {
//                Resource = Send.Resource,
//            }
//            .Property(Send.FromEmail, _options.SenderEmail)
//            .Property(Send.FromName, _options.SenderName)
//            .Property(Send.Subject, subject)
//            .Property(Send.HtmlPart, body)
//            .Property(Send.Recipients, new JArray
//            {
//            new JObject
//            {
//                {"Email", toEmail}
//            }
//            });

//            var response = await client.PostAsync(request);
//            if (!response.IsSuccessStatusCode)
//            {
//                throw new Exception($"Failed to send email. Status: {response.StatusCode}, Message: {response.GetErrorMessage()}");
//            }
//        }
//        public async Task SendPasswordResetEmailAsync(User user)
//        {
//            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
//            string resetLink = $"http://localhost:4200/reset-password?token={resetToken}&email={user.Email}"; // Adjust the URL as per your frontend route
//            string subject = "Reset Your Password";
//            string body = $"<p>Please reset your password by clicking <a href='{resetLink}'>here</a></p>";

//            await SendEmailAsync(user.Email, subject, body);
//        }
//    }
//}
