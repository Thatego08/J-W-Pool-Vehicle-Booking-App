/*using iText.Kernel.Counter.Context;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Data;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Services
{
    public class RateRepoService : IRateRepo
    {
        private readonly AppDbContext _context;

        public RateRepoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RateViewModel>> GetAllRatesAsync()
        {
            return await _context.Rates
                .Include(r => r.RateType)
                .Include(r => r.Project)
                .Select(r => new RateViewModel
                {
                 
                    RateTypeName = r.RateType.RateTypeName,
                    RateValue = r.RateValue,
                    ProjectNumber = r.Project.ProjectNumber,
                    ApplicableTimePeriod = r.ApplicableTimePeriod,
                    Conditions = r.Conditions
                })
                .ToListAsync();
        }

        public async Task<Rate> GetRateByIdAsync(int rateId)
        {
            return await _context.Rates.FindAsync(rateId);
        }

        public async Task<Rate> CreateRateAsync(Rate rate)
        {
            _context.Rates.Add(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

        public async Task<Rate> UpdateRateAsync(Rate rate)
        {
            _context.Rates.Update(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

    }
}*/