using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Team34FinalAPI.Models
{
    public class AdminRepo : IAdminRepo
    {
        private readonly UserDbContext _userDbContext;

        public AdminRepo(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<User[]> GetAllAdminsAsync()
        {
            IQueryable<User> query = _userDbContext.Users
                .Where(u => _userDbContext.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == _userDbContext.Roles.SingleOrDefault(r => r.Name == "Admin").Id));

            return await query.ToArrayAsync();
        }

        public async Task<User> GetAdminAsync(string userName)
        {
            var admin = await _userDbContext.Users
                .Where(a => a.UserName == userName && _userDbContext.UserRoles.Any(ur => ur.UserId == a.Id && ur.RoleId == _userDbContext.Roles.SingleOrDefault(r => r.Name == "Admin").Id))
                .FirstOrDefaultAsync();

            return admin;
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