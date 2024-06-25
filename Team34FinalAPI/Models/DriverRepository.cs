using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class DriverRepository : IDriverRepository
    {
        private readonly UserDbContext _userDbContext;

        public DriverRepository(UserDbContext userDbContext)
        {  
            _userDbContext = userDbContext;
        }

        public async Task<Driver[]> GetAllDriverAsync()
        {
            IQueryable<Driver> query = _userDbContext.Drivers.OfType<Driver>();
            return await query.ToArrayAsync();
        }

        public async Task<Driver> GetDriverAsync(string userName)
        {
            // Corrected parameter from courseId to userName and added OfType<Driver>()
            IQueryable<Driver> query = _userDbContext.Drivers.OfType<Driver>().Where(c => c.UserName ==userName);
            return await query.FirstOrDefaultAsync();
        }
        public void Add<T>(T entity) where T : class
        {
            _userDbContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _userDbContext.Remove(entity);
        }

        public async Task<bool> SaveChangesAync()
        {
            return (await _userDbContext.SaveChangesAsync()) > 0;
        }
    }
}
