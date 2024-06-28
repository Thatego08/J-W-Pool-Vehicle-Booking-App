using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Team34FinalAPI.Models
{
    public class TripRepository : ITripRepository
    {
        private readonly TripDbContext _context;

        public TripRepository(TripDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await _context.Trips.Include(t => t.TripMedia).ToListAsync();
        }

        public async Task<Trip> GetTripByIdAsync(int tripId)
        {
            return await _context.Trips.Include(t => t.TripMedia).FirstOrDefaultAsync(t => t.TripId == tripId);
        }

        public async Task AddTripAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
