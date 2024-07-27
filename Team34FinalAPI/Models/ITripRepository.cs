using System.Collections.Generic;
using System.Threading.Tasks;

namespace Team34FinalAPI.Models
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(int tripId);
      
        Task AddTripAsync(Trip trip);
        Task<bool> SaveChangesAsync();
        void Delete<T>(T entity) where T : class;
        Task<Trip> CreateTripAsync(Trip trip);
       
    }
}
