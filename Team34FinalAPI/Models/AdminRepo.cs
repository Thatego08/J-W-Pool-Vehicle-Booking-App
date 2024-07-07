using Microsoft.EntityFrameworkCore;
using System;

namespace Team34FinalAPI.Models
{
    public class AdminRepo
    {
        private readonly UserDbContext _userDbContext;

        public AdminRepo(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public async Task<User[]> GetAllAdminAsync()
        {
            // Query ApplicationUser with Admin role
            IQueryable<User> query = _userDbContext.Users
                .OfType<User>()
                .Where(u => _userDbContext.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == _userDbContext.Roles.SingleOrDefault(r => r.Name == "Admin").Id));

            return await query.ToArrayAsync();
        }

        public async Task<User> GetAdmin(string userName)
        {
            // Query for a specific Admin by username
            var admin = await _userDbContext.Users
                .OfType<User>()
                .Where(u => u.UserName == userName && _userDbContext.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == _userDbContext.Roles.SingleOrDefault(r => r.Name == "Admin").Id))
                .FirstOrDefaultAsync();

            return admin;
        }

        public async Task<bool> AddAdmin(User user)
        {
            try
            {
                await _userDbContext.Users.AddAsync(user);
                await _userDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAdmin(User user)
        {
            try
            {
                _userDbContext.Users.Update(user);
                await _userDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAdmin(string userName)
        {
            try
            {
                var admin = await GetAdmin(userName);
                if (admin != null)
                {
                    _userDbContext.Users.Remove(admin);
                    await _userDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

        }
