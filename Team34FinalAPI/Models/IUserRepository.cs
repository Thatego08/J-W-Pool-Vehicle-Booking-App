namespace Team34FinalAPI.Models
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //User
        Task<User[]> GetAllUsersAsync();
        Task<User> GetUserAsync(string UserName);
    }
}
