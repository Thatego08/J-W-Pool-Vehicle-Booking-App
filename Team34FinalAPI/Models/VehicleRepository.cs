using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Models
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _context;

        public VehicleRepository(VehicleDbContext context)
        {
            _context = context;
        }
        

        public async Task AddVehicleAsync(VehicleViewModel vvm)
        {
            var vehicle = new Vehicle()
            {
                VehicleID = vvm.VehicleID,
                Name = vvm.Name,
                Description = vvm.Description,
                DateAcquired = vvm.DateAcquired,
                RegistrationNumber = vvm.RegistrationNumber,
                VehicleMakeID = vvm.VehicleMakeID,
                VehicleModelID = vvm.VehicleModelID,
                VIN = vvm.VIN,
                StatusID = vvm.StatusID,
                ColourID = vvm.ColourID,
                InsuranceCoverID = vvm.InsuranceCoverID,
                FuelTypeID = vvm.FuelTypeID,
                LicenseExpiryDate   = vvm.LicenseExpiryDate,
                EngineNo = vvm.EngineNo,
            };

            var licenseDisk = new LicenseDisk()
            {
                LicenseExpiryDate = vvm.LicenseExpiryDate,
            };

            vehicle.LicenseDisk = licenseDisk;

            _context.Vehicles.Add(vehicle);
            _context.LicenseDisks.Add(licenseDisk);

            await _context.SaveChangesAsync();
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Vehicle[]> GetAllVehiclesAsync()
        {
            IQueryable<Vehicle> query = _context.Vehicles.
                Include(v => v.InsuranceCover).Include(v => v.VehicleMake).Include(v => v.VehicleModel).Include(v => v.FuelType).Include(v => v.Colour); 
            return await query.ToArrayAsync();
        }

        public async Task<Vehicle> GetVehicleAsync(int vehicleId)
        {
            return await _context.Vehicles.Include(v => v.InsuranceCover)
        .Include(v => v.Colour)
        .Include(v => v.FuelType)
        .Include(v => v.VehicleMake)
        .Include(v => v.VehicleModel).
                FirstOrDefaultAsync(v => v.VehicleID == vehicleId);
        }

        public async Task<InsuranceCover[]> GetInsuranceCoverAsync()
        {
            IQueryable<InsuranceCover> query = _context.InsuranceCover;
            return await query.ToArrayAsync();
        }

        public async Task<Status[]> GetAllStatusAsync()
        {
            IQueryable<Status> query = _context.Status;
            return await query.ToArrayAsync();
        }

        public async Task<VehicleFuelType[]> GetAllFuelTypeAsync()
        {
            IQueryable<VehicleFuelType> query = _context.FuelTypes;
            return await query.ToArrayAsync();
        }

        public async Task<Colour[]> GetAllColoursAsync()
        {
            IQueryable<Colour> query = _context.Colour;
            return await query.ToArrayAsync();
        }

        public async Task<LicenseDisk[]> GetAllLicenseDiskAsync()
        {
            IQueryable<LicenseDisk> query = _context.LicenseDisks;
            return await query.ToArrayAsync();
        }

        public async Task<VehicleChecklist[]> GetAllVehicleChecklistAsync()
        {
            IQueryable<VehicleChecklist> query = _context.VehicleChecklists;
            return await query.ToArrayAsync();
        }

        public async Task<VehicleModel[]> GetAllVehicleModelAsync()
        {
            IQueryable<VehicleModel> query = _context.VehicleModel.Include(vm => vm.VehicleMake);
            return await query.ToArrayAsync();
        }

        public async Task<VehicleMake[]> GetAllVehicleMakeAsync()
        {
            IQueryable<VehicleMake> query = _context.VehicleMake;
            return await query.ToArrayAsync();
        }

        public void AddColour(Colour colour)
        {
            _context.Add(colour);

        }

        public void AddFuelType(VehicleFuelType fuelType)
        {
            _context.Add(fuelType);
        }

        public void AddInsuranceCover(InsuranceCover insurance)
        {
            _context.Add(insurance);
        }

        public void AddVehicleMake(VehicleMake vehicleMake)
        {
            _context.Add(vehicleMake);
        }

        public void AddVehicleModel(VehicleModel vehicleModel)
        {
            _context.Add(vehicleModel);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

