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

        public async Task AddRefuelVehicleAsync(int tripId, RefuelVehicle refuelVehicle)
        {
            var trip = await _context.Trips.Include(t => t.RefuelVehicles).FirstOrDefaultAsync(t => t.TripId == tripId);
            if (trip != null)
            {
                trip.RefuelVehicles.Add(refuelVehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RefuelVehicle> GetRefuelVehicleByIdAsync(int tripId, int refuelVehicleId)
        {
            return await _context.RefuelVehicles.FirstOrDefaultAsync(rv => rv.TripId == tripId && rv.RefuelVehicleId == refuelVehicleId);
        }

        public async Task UpdateRefuelVehicleAsync(RefuelVehicle refuelVehicle)
        {
            _context.RefuelVehicles.Update(refuelVehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRefuelVehicleAsync(RefuelVehicle refuelVehicle)
        {
            _context.RefuelVehicles.Remove(refuelVehicle);
            await _context.SaveChangesAsync();
        }
    
public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
