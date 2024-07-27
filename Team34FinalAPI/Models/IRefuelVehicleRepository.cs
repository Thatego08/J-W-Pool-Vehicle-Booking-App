namespace Team34FinalAPI.Models
{
    public interface IRefuelVehicleRepository
    {
        Task<IEnumerable<RefuelVehicle>> GetAllAsync();
        Task<RefuelVehicle> GetByIdAsync(int id);
        Task AddAsync(RefuelVehicle refuelVehicle);
        Task UpdateAsync(RefuelVehicle refuelVehicle);
        Task DeleteAsync(int id);
        Task<bool> CheckIfTripExists(int tripId);
    }
}
