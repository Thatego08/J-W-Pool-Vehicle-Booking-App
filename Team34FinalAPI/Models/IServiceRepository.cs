using System.Threading.Tasks;

namespace Team34FinalAPI.Models
{
    public interface IServiceRepository
    {
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        Task<Service[]> GetAllServicesAsync();
        Task<Service> GetServiceAsync(int serviceId); // Match the implementation
    }
}

