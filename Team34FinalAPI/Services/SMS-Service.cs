using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Configuration;


namespace Team34FinalAPI.Services
{
    public class SMS_Service:ISMS_Service
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SMS_Service(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"];

            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            var messageOptions = new CreateMessageOptions(new PhoneNumber(toPhoneNumber))
            {
                From = new PhoneNumber(_fromPhoneNumber),
                Body = message
            };

            var msg = await MessageResource.CreateAsync(messageOptions);
        }
    }
}
