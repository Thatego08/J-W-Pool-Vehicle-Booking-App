using System.Collections.Generic;
using System.Threading.Tasks;

namespace Team34FinalAPI.Models
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(int tripId);
        Task<IEnumerable<Trip>> GetTripsByDriverIdAsync(int driverId);
        Task AddTripAsync(Trip trip);
        Task<bool> SaveChangesAsync();
    }
}
