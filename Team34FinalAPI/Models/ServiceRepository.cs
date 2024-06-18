using Microsoft.EntityFrameworkCore;


namespace Team34FinalAPI.Models
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly VehicleDbContext _context;

        public ServiceRepository(VehicleDbContext context)
        {
            _context = context;
        }


        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<Service[]> GetAllServicesAsync()
        {
            IQueryable<Service> query = _context.VehicleService;
            return await query.ToArrayAsync();
        }


        public async Task<Service> GetServiceAsync(int serviceId)
        {
            return await _context.VehicleService.FirstOrDefaultAsync(s => s.ServiceID == serviceId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
