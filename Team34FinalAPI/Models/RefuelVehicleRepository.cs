using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class RefuelVehicleRepository : IRefuelVehicleRepository
    {
        private readonly TripDbContext _context;

        public RefuelVehicleRepository(TripDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RefuelVehicle>> GetAllAsync()
        {
            return await _context.RefuelVehicles.ToListAsync();
        }
        public async Task<bool> CheckIfTripExists(int tripId)
        {
            return await _context.Trips.AnyAsync(t => t.TripId == tripId);
        }


        public async Task<RefuelVehicle> GetByIdAsync(int id)
        {
            return await _context.RefuelVehicles.FindAsync(id);
        }

        public async Task AddAsync(RefuelVehicle refuelVehicle)
        {
            _context.RefuelVehicles.Add(refuelVehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefuelVehicle refuelVehicle)
        {
            _context.Entry(refuelVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var refuelVehicle = await _context.RefuelVehicles.FindAsync(id);
            if (refuelVehicle != null)
            {
                _context.RefuelVehicles.Remove(refuelVehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}