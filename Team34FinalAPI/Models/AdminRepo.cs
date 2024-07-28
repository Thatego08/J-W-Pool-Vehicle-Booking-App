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
            // Query ApplicationUser with Admin role
            IQueryable<User> query = _userDbContext.Users
                .Where(u => _userDbContext.UserRoles
                    .Any(ur => ur.UserId == u.Id &&
                               ur.RoleId == _userDbContext.Roles
                               .SingleOrDefault(r => r.Name == "Admin").Id));

            return await query.ToArrayAsync();
        }

        public async Task<User> GetAdminAsync(string userName)
        {
            // Query for a specific Admin by username
            var admin = await _userDbContext.Users
                .Where(a => a.UserName == userName &&
                            _userDbContext.UserRoles
                            .Any(ur => ur.UserId == a.Id &&
                                       ur.RoleId == _userDbContext.Roles
                                       .SingleOrDefault(r => r.Name == "Admin").Id))
                .FirstOrDefaultAsync();

            return admin;
        }

        public void Delete(User admin)
        {
            _userDbContext.Remove(admin);
        }

        public async Task<bool> SaveChangesAync()
        {
            return (await _userDbContext.SaveChangesAsync()) > 0;
        }

        Task<IEnumerable<User>> IAdminRepo.GetAllAdminsAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(User admin)
        {
            throw new NotImplementedException();
        }
    }
}