using System.Collections.Generic;
using System.Threading.Tasks;

namespace Team34FinalAPI.Models
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<Service> GetServiceByIdAsync(int serviceId);

        Task<IEnumerable<Service>> GetServiceByAdminAsync(int adminId);

        Task CreateService(Service service);
        Task<bool> SaveChangesAsync();
    }
}

