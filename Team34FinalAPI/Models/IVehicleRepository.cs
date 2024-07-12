using Team34FinalAPI.ViewModels;
namespace Team34FinalAPI.Models
{
    public interface IVehicleRepository
    {
        Task<bool> SaveChangesAsync();
        Task AddVehicleAsync(VehicleViewModel vehicleModel);
        Task AddChecklistAsync(VehicleChecklist checklist);
        void Delete<T>(T entity) where T : class;

        void AddVehicleMake(VehicleMake vehicleMake);
        void AddVehicleModel (VehicleModel vehicleModel);
        void AddColour(Colour colour);
        void AddInsuranceCover (InsuranceCover cover);
        void AddFuelType(VehicleFuelType fuelType);
        Task<Vehicle[]> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleAsync(int vehicleId);
        Task<VehicleMake[]> GetAllVehicleMakeAsync();
        Task<VehicleFuelType[]> GetAllFuelTypeAsync();
        Task<Colour[]> GetAllColoursAsync();
        Task<InsuranceCover[]> GetInsuranceCoverAsync();
        Task<LicenseDisk[]> GetAllLicenseDiskAsync();
        Task<VehicleChecklist[]> GetAllVehicleChecklistAsync();
        Task<Status[]> GetAllStatusAsync();
        Task<VehicleModel[]> GetAllVehicleModelAsync();    

    }
}
