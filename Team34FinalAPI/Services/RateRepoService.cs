using iText.Kernel.Counter.Context;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Data;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public class RateRepoService : IRateRepo
    {
        private readonly AppDbContext _db;

        public RateRepoService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateRate(Rate rate)
        {
            try
            {
                await _db.Rates.AddAsync(rate);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating rate: {ex.Message}");
                return false;
            }
        }

        public async Task<Rate> GetRate(int rateId)
        {
            return await _db.Rates
                .Include(r => r.Project)
                .Include(r => r.RateType)
                .FirstOrDefaultAsync(r => r.RateID == rateId);
        }

        public async Task<bool> UpdateRate(Rate rate)
        {
            try
            {
                _db.Rates.Update(rate);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating rate: {ex.Message}");
                return false;
            }
        }

        public async Task<Rate> AddRateAsync(Rate rate)
        {
            _db.Rates.Add(rate);
            await _db.SaveChangesAsync();
            return rate;
        }
    }
}
