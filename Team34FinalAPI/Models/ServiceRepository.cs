using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Team34FinalAPI.Models
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly VehicleDbContext _context;

        public ServiceRepository(VehicleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await _context.VehicleService.Include(s => s.VehicleID).ToListAsync();
        }

        public async Task<Service> GetServiceByIdAsync( int serviceId)
        {
            return await _context.VehicleService.Include(s => s.AdminName).FirstOrDefaultAsync(s => s.ServiceID == serviceId);
        }

        public async Task<IEnumerable<Service>> GetServiceByAdminAsync(int adminId)
        {
            return await _context.VehicleService.Include(s => s.VehicleID).
                Include(s => s.VehicleMakeName).Include(s => s.VehicleModelName).
                Include(s => s.AdminName).
                Include(s => s.AdminEmail).ToListAsync();
        }

        public async Task CreateService(Service service)
        {
            await _context.VehicleService.AddAsync(service);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
