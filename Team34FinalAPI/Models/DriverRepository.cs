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

        public async Task<User[]> GetAllDriverAsync()
        { 
            // Query ApplicationUser with Driver role
            IQueryable<User> query = _userDbContext.Drivers
                .OfType<User>()
                .Where(u => _userDbContext.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == _userDbContext.Roles.SingleOrDefault(r => r.Name == "Driver").Id));

            return await query.ToArrayAsync();
        }

        public async Task<Driver> GetDriverAsync(string userName)
        {
            // Query for a specific Driver by username
            var driver = await _userDbContext.Drivers
                .OfType<Driver>()
                .Where(d => d.UserName == userName && _userDbContext.UserRoles.Any(ur => ur.UserId == d.Id && ur.RoleId == _userDbContext.Roles.SingleOrDefault(r => r.Name == "Driver").Id))
                .FirstOrDefaultAsync();
            //IQueryable<Driver> query = _userDbContext.Drivers.OfType<Driver>().Where(c => c.UserName ==userName);
            return driver;
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
