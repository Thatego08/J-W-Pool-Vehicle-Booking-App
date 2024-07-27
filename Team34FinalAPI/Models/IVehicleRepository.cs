using Team34FinalAPI.ViewModels;
namespace Team34FinalAPI.Models
{
    public interface IVehicleRepository
    {
        Task<bool> SaveChangesAsync();
        Task AddVehicleAsync(VehicleViewModel vehicleModel);
        
        void Delete<T>(T entity) where T : class;

        void AddVehicleMake(VehicleMake vehicleMake);
        void AddVehicleModel (VehicleModel vehicleModel);
        void AddColour(Colour colour);
        void AddInsuranceCover (InsuranceCover cover);
        void AddFuelType(VehicleFuelType fuelType);
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetAvailableVehicles();
        Task<Vehicle> GetVehicleAsync(int vehicleId);
        Task<VehicleMake[]> GetAllVehicleMakeAsync();
        Task<VehicleFuelType[]> GetAllFuelTypeAsync();
        Task<Colour[]> GetAllColoursAsync();
        Task<InsuranceCover[]> GetInsuranceCoverAsync();
        Task<LicenseDisk[]> GetAllLicenseDiskAsync();
        Task<IEnumerable<VehicleChecklist>> GetChecklistsAsync();

        Task<Status[]> GetAllStatusAsync();
        Task<VehicleModel[]> GetAllVehicleModelAsync();

        Task<Colour> GetColourAsync(int colourId);
        Task<InsuranceCover> GetInsuranceAsync(int Id);
        Task<VehicleFuelType> GetFuelAsync(int Id);
        Task<VehicleMake> GetMakeAsync(int Id);
        Task<VehicleModel> GetModelAsync(int Id);

    }
}
