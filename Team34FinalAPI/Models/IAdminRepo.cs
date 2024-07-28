
namespace Team34FinalAPI.Models
{
    public interface IAdminRepo
    {


        Task<User[]> GetAllAdminsAsync();
        Task<User> GetAdminAsync(string userName);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<bool> SaveChangesAync();
    }
}
