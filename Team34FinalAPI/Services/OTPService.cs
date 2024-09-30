using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Team34FinalAPI.Models;
using static System.Net.WebRequestMethods;

namespace Team34FinalAPI.Services
{
    public class OTPService :IOTPService
    {

        private readonly IOTPRepository _otpRepository;
        private readonly int _otpExpiryMinutes = 10;  // Set OTP expiration time (10 minutes)
        private readonly UserDbContext _context;
        private readonly OTPSettings _otpSettings;
        public OTPService(IOTPRepository otpRepository, UserDbContext context, IOptions<OTPSettings> otpSettings)
        {
            _otpRepository = otpRepository;
            _context = context;
            _otpSettings = otpSettings.Value;
        }

        public async Task<string> GenerateAndSaveOtpAsync(string email)
        {
            var otp = GenerateOtp(6);
            var expiryTime = DateTime.UtcNow.AddMinutes(_otpSettings.ExpirationTimeInMinutes);

            var otpEntity = new OTP
            {
                Email = email,
                Code = otp,
                ExpiryTime = expiryTime,
                IsUsed = false
            };

            await _otpRepository.SaveOtpAsync(otpEntity);
            return otp;
        }

        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            var otpEntity = await _otpRepository.GetOtpAsync(email);
            if (otpEntity == null || otpEntity.IsUsed)
            {
                return false;
            }

            if (otpEntity.ExpiryTime < DateTime.UtcNow)
            {
                return false;
            }

            var isValid = otpEntity.Code == otp;
            if (isValid)
            {
                await _otpRepository.MarkOtpAsUsedAsync(email);
            }

            return isValid;
        }

        public async Task MarkOtpAsUsedAsync(string email)
        {
            await _otpRepository.MarkOtpAsUsedAsync(email);
        }
        public async Task<OTP> GetOtpAsync(string email)
        {
            return await _context.Otps
        .Where(o => o.Email == email && !o.IsUsed)
        .OrderByDescending(o => o.ExpiryTime)
        .FirstOrDefaultAsync();
        }

        private string GenerateOtp(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
