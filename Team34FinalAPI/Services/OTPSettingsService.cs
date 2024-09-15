using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public class OTPSettingsService
    {
        private readonly UserDbContext _context;

        public OTPSettingsService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetOtpExpirationTimeAsync()
        {
            var settings = await _context.OTPSettings.FirstOrDefaultAsync();
            return settings?.ExpirationTimeInMinutes ?? 10; // Default to 10 minutes if not set
        }

        public async Task UpdateOtpExpirationTimeAsync(int newExpirationTime)
        {
            var settings = await _context.OTPSettings.FirstOrDefaultAsync();
            if (settings != null)
            {
                settings.ExpirationTimeInMinutes = newExpirationTime;
                _context.OTPSettings.Update(settings);
                await _context.SaveChangesAsync();
            }
        }
    }
}
