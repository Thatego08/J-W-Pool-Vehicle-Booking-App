
namespace Team34FinalAPI.Models
{
    public interface IAdminRepo
    {

        // Admin
        Task<User[]> GetAllAdminAsync();
        Task<bool> AddAdmin(User UserName);
        Task<User> GetAdmin(string UserName);
        Task<bool> UpdateAdmin(User user);
        Task<bool> DeleteAdmin(string UserName);
    }
}
