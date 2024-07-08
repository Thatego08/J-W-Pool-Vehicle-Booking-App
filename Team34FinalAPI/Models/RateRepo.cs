using static Team34FinalAPI.Models.RateRepo;
using System;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Models
{
    public class RateRepo : IRateRepo
    {
        private readonly UserDbContext _userDbContext;

        public RateRepo (UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<bool> CreateRate(Rate rate)
        {
            try
            {
                await _userDbContext.Rates.AddAsync(rate);
                await _userDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateRate(Rate rate)
        {
            try
            {
                _userDbContext.Rates.Update(rate);
                await _userDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
