using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Data;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Models
{
    public class RateRepository : IRateRepo
    {

        private readonly AppDbContext _context;

        public RateRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all rates along with their Project and RateType details
        public async Task<IEnumerable<RateViewModel>> GetAllRatesAsync()
        {
            return await _context.Rates
                .Include(r => r.Project)
                .Select(r => new RateViewModel
                {
                    ProjectID = r.ProjectID,
                    RateValue = r.RateValue,
                    ApplicableTimePeriod = r.ApplicableTimePeriod,
                    Conditions = r.Conditions
                })
                .ToListAsync();
        }

        // Get a rate by its ID with Project and RateType included
        public async Task<RateViewModel> GetRateByIdAsync(int rateId)
        {
            return await _context.Rates
                .Include(r => r.Project)
                .Where(r => r.RateID == rateId)
                .Select(r => new RateViewModel
                {
                    ProjectID = r.ProjectID,
                    RateValue = r.RateValue,
                    ApplicableTimePeriod = r.ApplicableTimePeriod,
                    Conditions = r.Conditions
                })
                .FirstOrDefaultAsync();
        }

        // Create a new rate
        public async Task<Rate> CreateRateAsync(Rate rate)
        {
            _context.Rates.Add(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

        // Update an existing rate
        public async Task<Rate> UpdateRateAsync(Rate rate)
        {
            var existingRate = await _context.Rates.FindAsync(rate.RateID);
            if (existingRate != null)
            {
                _context.Entry(existingRate).State = EntityState.Modified;

                existingRate.ProjectID = rate.ProjectID;
                existingRate.RateValue = rate.RateValue;
                existingRate.ApplicableTimePeriod = rate.ApplicableTimePeriod;
                existingRate.Conditions = rate.Conditions;

                await _context.SaveChangesAsync();
            }
            return existingRate;
        }
    }
}
