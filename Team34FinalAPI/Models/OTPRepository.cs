using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace Team34FinalAPI.Models
{
    public class OTPRepository :IOTPRepository
    {
        private readonly UserDbContext _context;

        public OTPRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task SaveOtpAsync(OTP otp)
        {
            _context.Otps.Add(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<OTP> GetOtpAsync(string email)
        {
            return await _context.Otps.FirstOrDefaultAsync(o => o.Email == email && !o.IsUsed);
        }

        public async Task MarkOtpAsUsedAsync(string email)
        {
            var otp = await GetOtpAsync(email);
            if (otp != null)
            {
                otp.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
