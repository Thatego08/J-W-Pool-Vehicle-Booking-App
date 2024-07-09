using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class UserRepository: IUserRepository
    {
        private readonly UserDbContext _userDbContext;
        private readonly UserManager<User> _userManager;

        public UserRepository(UserDbContext userDbContext, UserManager<User> userManager)
        {
            _userDbContext = userDbContext;
            _userManager = userManager;
        }

        public void Add<T>(T entity) where T : class
        {
            _userDbContext.Set<T>().Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _userDbContext.Remove(entity);
        }

        public async Task<User[]> GetAllUsersAsync()
        {
            IQueryable<User> query = _userDbContext.Users;
            return await query.ToArrayAsync();
        }

        public async Task<User> GetUserAsync(string userName)
        {
            IQueryable<User> query = _userDbContext.Users.Where(u => u.UserName == userName);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _userDbContext.SaveChangesAsync() > 0;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
