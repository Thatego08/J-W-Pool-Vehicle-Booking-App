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
            // Fetch trips without including TripMedia
            return await _context.Trips.ToListAsync();
        }


        public async Task<Trip> GetTripByIdAsync(int tripId)
        {
            // Fetch a trip by its ID without including TripMedia
            return await _context.Trips
                                 .FirstOrDefaultAsync(t => t.TripId == tripId);
        }



        public async Task AddTripAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
        }

        public async Task<Trip> CreateTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            return trip;
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

       
    
public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
