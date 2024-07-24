
namespace Team34FinalAPI.Models
{
    public interface IAdminRepo
    {

        // Admin
        Task<User[]> GetAllAdminAsync(); //R
        Task<bool> AddAdmin(User UserName); //C
        Task<User> GetAdmin(string UserName); //R-One
        Task<bool> UpdateAdmin(User user); // U
        Task<bool> DeleteAdmin(string UserName); //D
    }
}
