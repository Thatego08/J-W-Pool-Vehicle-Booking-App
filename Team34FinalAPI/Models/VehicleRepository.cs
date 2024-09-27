using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using Org.BouncyCastle.Crypto;

namespace Team34FinalAPI.Models
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _context;
        private readonly BookingDbContext _bContext;

        public VehicleRepository(VehicleDbContext context, BookingDbContext bContext)
        {
            _context = context;
            _bContext = bContext;
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
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

        //Additions

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync(DateTime startDate, DateTime endDate)
        {
            // Get the IDs of vehicles that have conflicting bookings
            var unavailableVehicleIds = await _bContext.Bookings
                .Where(b => (b.StartDate < endDate && b.EndDate > startDate)) // Conflict in date ranges
                .Select(b => b.VehicleId)
                .ToListAsync();

            // Return all vehicles that are not in the unavailable list
            return await _context.Vehicles
                .Where(v => !unavailableVehicleIds.Contains(v.VehicleID)) // Available vehicles not booked
                .ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByNameAsync(string name)
        {
            return await _context.Vehicles.SingleOrDefaultAsync(v => v.Name == name);
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            IQueryable<Vehicle> query = _context.Vehicles
                .Include(v => v.InsuranceCover)
                .Include(v => v.VehicleMake)
                .Include(v => v.VehicleModel)
                .Include(v => v.FuelType)
                .Include(v => v.Colour)
                .Include(v => v.Status)
                .Include(v => v.LicenseDisk);
            return await query.ToArrayAsync();
        }

        public async Task<Vehicle> GetVehicleAsync(int vehicleId)
        {
            return await _context.Vehicles
             .Include(v => v.InsuranceCover)
             .Include(v => v.Colour)
             .Include(v => v.FuelType)
             .Include(v => v.VehicleMake)
             .Include(v => v.VehicleModel)
             .Include(v => v.Status)
             .Include(v => v.LicenseDisk)
             .FirstOrDefaultAsync(v => v.VehicleID == vehicleId);
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehicles()
        {
            return await _context.Vehicles
                .Where(v => v.StatusID == 1) // Assuming StatusId = 1 means available
                .Include(v => v.InsuranceCover)
                .Include(v => v.VehicleMake)
                .Include(v => v.VehicleModel)
                .Include(v => v.FuelType)
                .Include(v => v.Colour)
                .Include(v => v.Status)
                .Include(v => v.LicenseDisk)
                .ToListAsync();
        }

        /*    public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
            {
                return await _context.Vehicles
                    .Include(v => v.Colour)
                    .Include(v => v.FuelType)
                    .Include(v => v.VehicleMake)
                    .Include(v => v.VehicleModel)
                    .Include(v => v.Status)
                    .FirstOrDefaultAsync(v => v.VehicleID == vehicleId);
            }

    */

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);

            // Get the current date
            var currentDate = DateTime.UtcNow;

            // Check if the vehicle is booked for the current date
            var activeBooking = await _bContext.Bookings
                .Where(b => b.VehicleId == vehicleId && b.StartDate <= currentDate && b.EndDate >= currentDate)
                .FirstOrDefaultAsync();

            // If there's an active booking, set status to 'Booked', otherwise 'Available'
            vehicle.StatusID = activeBooking != null ? 2 : 1; // 2 for 'Booked', 1 for 'Available'

            return vehicle;
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

        public async Task<IEnumerable<VehicleChecklist>> GetChecklistsAsync()
        {
            return await _context.VehicleChecklists.ToListAsync();
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

        /*Task<Colour> GetColourAsync(int colourId);
        Task<InsuranceCover> GetInsuranceAsync(int Id);
        Task<VehicleFuelType> GetFuelAsync(int Id);
        Task<VehicleMake> GetMakeAsync(int Id);
        Task<VehicleModel> GetModelAsync(int Id); */

        public async Task<Colour> GetColourAsync(int colourId)
        {
            return await _context.Colour
             .FirstOrDefaultAsync(v => v.Id == colourId);
        }

        public async Task<InsuranceCover> GetInsuranceAsync(int Id)
        {
            return await _context.InsuranceCover
             .FirstOrDefaultAsync(v => v.InsuranceCoverId == Id);
        }

        public async Task<VehicleFuelType> GetFuelAsync(int Id)
        {
            return await _context.FuelTypes
             .FirstOrDefaultAsync(v => v.Id == Id);
        }

        public async Task<VehicleMake> GetMakeAsync(int Id)
        {
            return await _context.VehicleMake
             .FirstOrDefaultAsync(v => v.VehicleMakeID == Id);
        }

        public async Task<VehicleModel> GetModelAsync(int Id)
        {
            return await _context.VehicleModel.Include(v => v.VehicleModelName)
             .FirstOrDefaultAsync(v => v.VehicleModelID == Id);
        }

        public async Task<List<VehicleModel>> GetModelsByMakeAsync(int makeId)
        {
            return await _context.VehicleModel
                .Where(v => v.VehicleMakeID == makeId)
                .Include(v => v.VehicleMake) // Include the VehicleMake details
                .ToListAsync(); // Return a list of vehicle models
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

