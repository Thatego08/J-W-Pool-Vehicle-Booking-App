using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Data;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Models
{
    public class RateRepository:IRateRepo
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
                  .Include(r => r.ProjectRates) // Include the join table
                    .ThenInclude(pr => pr.Project) 
                .Include(r => r.RateType)
                .Select(r => new RateViewModel
                {

                    ProjectNumbers = r.ProjectRates.Select(pr => pr.Project.ProjectNumber).ToList(),
                    RateTypeName = r.RateType.RateTypeName,
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
                .Include(r => r.ProjectRates)
                .ThenInclude(pr => pr.Project) // Then include the related Project

                .Include(r => r.RateType)
                .Where(r => r.RateID == rateId)
                .Select(r => new RateViewModel
                {
                    ProjectNumbers = r.ProjectRates.Select(pr => pr.Project.ProjectNumber).ToList(),
                    RateTypeName = r.RateType.RateTypeName,
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
        public async Task AddRateToProjectAsync(int rateId, int projectId)
        {
            var rate = await _context.Rates.FindAsync(rateId);
            var project = await _context.Projects.FindAsync(projectId);

            if (rate == null || project == null)
            {
                throw new Exception("Rate or Project not found.");
            }

            // Add new ProjectRate relationship
            var projectRate = new ProjectRate
            {
                RateID = rateId,
                ProjectID = projectId
            };

            _context.ProjectRates.Add(projectRate);
            await _context.SaveChangesAsync();
        }

        // Update an existing rate
        public async Task<Rate> UpdateRateAsync(Rate rate)
        {
            var existingRate = await _context.Rates.FindAsync(rate.RateID);
            if (existingRate != null)
            {
                
                existingRate.RateTypeID = rate.RateTypeID;
                existingRate.RateValue = rate.RateValue;
                existingRate.ApplicableTimePeriod = rate.ApplicableTimePeriod;
                existingRate.Conditions = rate.Conditions;

                await _context.SaveChangesAsync();
            }
            return existingRate;
        }
    }
}
