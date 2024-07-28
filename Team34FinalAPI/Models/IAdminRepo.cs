
namespace Team34FinalAPI.Models
{
    public interface IAdminRepo
    {

        Task<IEnumerable<User>> GetAllAdminsAsync();
        Task<User> GetAdminAsync(string userName);
        void Update(User admin);
        void Delete(User admin);
        Task<bool> SaveChangesAync();
    }
}
