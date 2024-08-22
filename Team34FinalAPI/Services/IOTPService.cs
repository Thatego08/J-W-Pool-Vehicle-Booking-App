using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public interface IOTPService
    {
        Task<string> GenerateAndSaveOtpAsync(string email);
        Task<bool> ValidateOtpAsync(string email, string otp);


        Task<OTP> GetOtpAsync(string email);
        Task MarkOtpAsUsedAsync(string email);
    }
}
