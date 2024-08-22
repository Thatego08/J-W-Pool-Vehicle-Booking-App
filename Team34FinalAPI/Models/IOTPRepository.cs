using static System.Net.WebRequestMethods;

namespace Team34FinalAPI.Models
{
    public interface IOTPRepository
    {
        Task SaveOtpAsync(OTP otp);
        Task<OTP> GetOtpAsync(string email);
        Task MarkOtpAsUsedAsync(string email);
    }
}
