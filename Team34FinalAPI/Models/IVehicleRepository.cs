namespace Team34FinalAPI.Models
{
    public interface IVehicleRepository
    {
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Vehicle[]> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleAsync(int vehicleId);
    }
}
