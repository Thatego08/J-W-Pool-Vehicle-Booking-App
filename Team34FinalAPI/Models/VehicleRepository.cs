using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Models
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _context;

        public VehicleRepository(VehicleDbContext context)
        {
            _context = context;
        }


        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Vehicle[]> GetAllVehiclesAsync()
        {
            IQueryable<Vehicle> query = _context.Vehicles;
            return await query.ToArrayAsync();
        }

        public async Task<Vehicle> GetVehicleAsync(int vehicleId)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleID == vehicleId);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

