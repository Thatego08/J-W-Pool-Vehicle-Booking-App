using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;


namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<VehicleController> _logger;
        private readonly IAuditLogRepository _auditLogRepo;
        private readonly VehicleDbContext _vehicleContext;


        public VehicleController(IVehicleRepository vehicleRepository, IWebHostEnvironment environment, ILogger<VehicleController> logger, IAuditLogRepository auditLogRepo, VehicleDbContext context)
        {
            _vehicleRepository = vehicleRepository;
            _environment = environment;
            _logger = logger;
            _auditLogRepo = auditLogRepo;
            _vehicleContext = context;

        }

       /* [Authorize(Roles = "Admin")]
       */
        
        [HttpGet("GetAllVehicles")]

        public async Task<IActionResult> GetAllVehicles()
        {
            /* try
             {
                 var results = await _vehicleRepository.GetAllVehiclesAsync();
                 return Ok(results);
             }
             catch (Exception ex )
             {
                 _logger.LogError(ex, "An error occured when retrieving vehicles");
                 return StatusCode(500, "Internal Server Error: Unable to retrieve vehicles.");
             }*/
            try
            {

                var vehicles = _vehicleContext.Vehicles
               .Include(v => v.Status)
               .Select(v => new VehicleViewModel
               {
                   VehicleID = v.VehicleID,
                   Name = v.Name,
                   Description = v.Description,
                   VehicleMakeID = v.VehicleMakeID,
                   VehicleModelID = v.VehicleModelID,
                   DateAcquired = v.DateAcquired,
                   LicenseExpiryDate = v.LicenseExpiryDate,
                   RegistrationNumber = v.RegistrationNumber,
                   InsuranceCoverID = v.InsuranceCoverID,
                   VIN = v.VIN,
                   EngineNo = v.EngineNo,
                   ColourID = v.ColourID,
                   FuelTypeID = v.FuelTypeID,
                   StatusID = v.StatusID,
                   VehicleType = v.VehicleType,

                   // New properties
                   CabinType = v.CabinType,
                   DriveType = v.DriveType,
                   Transmission = v.Transmission,
                   HasTowBar = v.HasTowBar,
                   HasCanopy = v.HasCanopy,
                   Compliance = v.Compliance ?? string.Empty,
                   Protection = v.Protection ?? string.Empty
               })
               .ToList();

                return Ok(vehicles);


            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, "Internal server error");
            }



        }

        [HttpGet("GetVehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleAsync(int vehicleId)
        {
            try
            {
                var result = await _vehicleRepository.GetVehicleAsync(vehicleId);
                if (result == null) return NotFound("Vehicle does not exist.");

                var insuranceCoverName = result.InsuranceCover.InsuranceCoverName;
                var colourName = result.Colour.Name;
                var fuelTypeName = result.FuelType.FuelName;
                var vehicleMakeName = result.VehicleMake.Name;
                var vehicleModelName = result.VehicleModel.VehicleModelName;
                return Ok(result);
            }
            catch (Exception )
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve the vehicle.");
            }
        }

        //Add Vehicle Adjustment
        [Authorize(Roles = "Admin")]
        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleViewModel vehicleViewModel)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    Name = vehicleViewModel.Name,
                    Description = vehicleViewModel.Description,
                    VehicleMakeID = vehicleViewModel.VehicleMakeID,
                    VehicleModelID = vehicleViewModel.VehicleModelID,
                    DateAcquired = vehicleViewModel.DateAcquired,
                    LicenseExpiryDate = vehicleViewModel.LicenseExpiryDate,
                    RegistrationNumber = vehicleViewModel.RegistrationNumber,
                    InsuranceCoverID = vehicleViewModel.InsuranceCoverID,
                    VIN = vehicleViewModel.VIN,
                    EngineNo = vehicleViewModel.EngineNo,
                    ColourID = vehicleViewModel.ColourID,
                    FuelTypeID = vehicleViewModel.FuelTypeID,
                    VehicleType = vehicleViewModel.VehicleType,

                    CabinType = vehicleViewModel.CabinType,
                    DriveType = vehicleViewModel.DriveType,
                    Transmission = vehicleViewModel.Transmission,
                    HasTowBar = vehicleViewModel.HasTowBar,
                    HasCanopy = vehicleViewModel.HasCanopy,


                    StatusID = 1
                };

                await _vehicleRepository.AddVehicleAsync(vehicle);

                var result = "Vehicle added successfully!";

                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Add Vehicle",
                    Details = "Vehicle has been successfully added by." + User.Identity.Name,
                    Timestamp = DateTime.UtcNow
                });
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding vehicle: {ex.Message}");
            }
        }


        /*[HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleViewModel vehicleViewModel)
        {
            try
            {
                await _vehicleRepository.AddVehicleAsync(vehicleViewModel);
                return Ok("Vehicle added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding vehicle: {ex.Message}");
            }
        }

        */
        

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditVehicle/{vehicleId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditVehicle(int vehicleId, VehicleViewModel vehicleModel)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetVehicleAsync(vehicleId);
                if (existingVehicle == null) return NotFound("The vehicle does not exist");

                existingVehicle.Name = vehicleModel.Name;
                existingVehicle.Description = vehicleModel.Description;
                existingVehicle.DateAcquired = vehicleModel.DateAcquired;
                existingVehicle.RegistrationNumber = vehicleModel.RegistrationNumber;
                existingVehicle.VehicleMakeID = vehicleModel.VehicleMakeID;
                existingVehicle.VehicleModelID = vehicleModel.VehicleModelID;
                existingVehicle.ColourID = vehicleModel.ColourID;
                existingVehicle.EngineNo = vehicleModel.EngineNo;
                existingVehicle.FuelTypeID = vehicleModel.FuelTypeID;
                existingVehicle.InsuranceCoverID = vehicleModel.InsuranceCoverID;
                existingVehicle.LicenseExpiryDate = vehicleModel.LicenseExpiryDate;
                existingVehicle.StatusID = vehicleModel.StatusID;
                existingVehicle.VIN = vehicleModel.VIN;
                existingVehicle.CabinType = vehicleModel.CabinType;
                existingVehicle.DriveType = vehicleModel.DriveType;
                existingVehicle.Transmission = vehicleModel.Transmission;
                existingVehicle.HasTowBar = vehicleModel.HasTowBar;
                existingVehicle.HasCanopy = vehicleModel.HasCanopy;

                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Edit Vehicle Details",
                        Details = "Vehicle details have been successfully updated by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return BadRequest("Your request is invalid");
        }

        // GET: api/vehicle/available
    /*    [HttpGet("GetAvailableVehicles")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles()
        {
            var availableVehicles = await _vehicleRepository.GetAvailableVehicles();

            if (availableVehicles == null || !availableVehicles.Any())
            {
                return NotFound("No available vehicles found.");
            }

            return Ok(availableVehicles);
        }*/

        // VehicleController.cs
        [HttpGet("GetAvailableVehicles")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string vehicleType = null)
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync(startDate, endDate, vehicleType);
                return Ok(vehicles);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available vehicles");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteVehicle/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetVehicleAsync(vehicleId);
                if (existingVehicle == null) return NotFound("The vehicle does not exist.");

                _vehicleRepository.Delete(existingVehicle);

                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Delete Vehicle",
                        Details = "Vehicle has been successfully deleted by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
                return BadRequest("Failed to delete the vehicle.");
            }
            catch (Exception )
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to delete the vehicle.");
            }
        }

        //
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllVehicleMakes")]
        public async Task<IActionResult> GetAllVehicleMakes()
        {
            try
            {
                var results = await _vehicleRepository.GetAllVehicleMakeAsync();
                return Ok(results);
            }
            catch (Exception )
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle makes.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddVehicleMake")]
        public async Task<IActionResult> AddVehicleMake([FromBody] VehicleMake vehicleMake)
        {
            try
            {
                if (vehicleMake == null)
                {
                    return BadRequest("Vehicle make is required.");
                }

                _vehicleRepository.AddVehicleMake(vehicleMake);
                await _vehicleRepository.SaveChangesAsync();

                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Add Vehicle Make",
                    Details = "Vehicle Make has been successfully added by." + User.Identity.Name,
                    Timestamp = DateTime.UtcNow
                });
                return CreatedAtAction(nameof(AddVehicleMake), vehicleMake);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add vehicle make.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetVehicleMake/{makeId}")]
        public async Task<IActionResult> GetMakeAsync(int makeId)
        {
            try
            {
                var result = await _vehicleRepository.GetMakeAsync(makeId);
                if (result == null) return NotFound("Vehicle make does not exist.");

                var vehicleMakeID = result.VehicleMakeID;
                return Ok(result);
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve the vehicle make.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditMake/{makeId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditMake(int makeId, VehicleMake vehicleMake)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetMakeAsync(makeId);
                if (existingVehicle == null) return NotFound("The vehicle make does not exist");

                existingVehicle.VehicleMakeID = vehicleMake.VehicleMakeID;
                existingVehicle.Name = vehicleMake.Name;
                

                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Edit Vehicle Make",
                        Details = "Vehicle Make has been successfully updated by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return BadRequest("Your request is invalid");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteMake/{makeId}")]
        public async Task<IActionResult> DeleteMake(int makeId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetMakeAsync(makeId);
                if (existingVehicle == null) return NotFound("The vehicle make does not exist.");

                _vehicleRepository.Delete(existingVehicle);

                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Delete Vehicle Make",
                        Details = "Vehicle Make has been successfully Deleted by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
                return BadRequest("Failed to delete the vehicle make.");
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to delete the vehicle make.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetModelByMake/{makeId}")]
        public async Task<IActionResult> GetModelsByMakeAsync(int makeId)
        {
            try
            {
                var result = await _vehicleRepository.GetModelsByMakeAsync(makeId);

                if (result == null || result.Count == 0)
                    return NotFound("No vehicle models exist for this make.");

                // Optionally, you can format the response to include relevant details
                var vehicleModels = result.Select(model => new
                {
                    model.VehicleModelID,
                    model.VehicleModelName
                });

                return Ok(vehicleModels);  // Returning the list of models
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle models.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllVehicleModels")]
        public async Task<IActionResult> GetAllVehicleModels()
        {
            try
            {
                var results = await _vehicleRepository.GetAllVehicleModelAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle models.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddVehicleModel")]
        public async Task<IActionResult> AddVehicleModel([FromBody] ModelViewModel mvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            try
            {
                if (mvm == null)
                {
                    return BadRequest("Vehicle model is required.");
                }

                mvm.VehicleModelID = 0; // Ensure ID is set to 0 for new entry

                _vehicleRepository.AddVehicleModelAsync(mvm);
                await _vehicleRepository.SaveChangesAsync();
                return CreatedAtAction(nameof(AddVehicleModel), new { id = mvm.VehicleModelID }, mvm);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, $"Internal Server Error: Unable to add vehicle model. {ex.Message}");
            }
        }



        /*
        [Authorize(Roles = "Admin")]
        [HttpPost("AddVehicleModel")]
        public async Task<IActionResult> AddVehicleModel([FromBody] VehicleModel vehicleModel)
        {
            try
            {
                if (vehicleModel == null)
                {
                    return BadRequest("Vehicle model is required.");
                }

                _vehicleRepository.AddVehicleModel(vehicleModel);
                await _vehicleRepository.SaveChangesAsync();

                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Add Vehicle Model",
                    Details = "Vehicle Model has been successfully added by." + User.Identity.Name,
                    Timestamp = DateTime.UtcNow
                });
                return CreatedAtAction(nameof(AddVehicleModel), vehicleModel);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add vehicle model.");
            }
        }*/

        [Authorize(Roles = "Admin")]
        [HttpGet("GetVehicleModel/{modelId}")]
        public async Task<IActionResult> GetModelAsync(int modelId)
        {
            try
            {
                var result = await _vehicleRepository.GetModelAsync(modelId);
                if (result == null) return NotFound("Vehicle model does not exist.");

                var vehicleModelID = result.VehicleModelID;
                var vehicleModelName = result.VehicleModelName;
                return Ok(result);
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve the vehicle model.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditModel/{modelId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditModel(int modelId, VehicleModel vehicleModel)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetModelAsync(modelId);
                if (existingVehicle == null) return NotFound("The vehicle make does not exist");

                existingVehicle.VehicleModelID = vehicleModel.VehicleModelID;
                existingVehicle.VehicleModelName = vehicleModel.VehicleModelName;
                existingVehicle.VehicleMakeID = vehicleModel.VehicleMakeID;
                existingVehicle.VehicleMake.Name = vehicleModel.VehicleMake.Name;


                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Edit Vehicle Model",
                        Details = "Vehicle Model has been successfully updated by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return BadRequest("Your request is invalid");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteModel/{modelId}")]
        public async Task<IActionResult> DeleteModel(int modelId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetModelAsync(modelId);
                if (existingVehicle == null) return NotFound("The vehicle model does not exist.");

                _vehicleRepository.Delete(existingVehicle);

                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Delete Vehicle Model",
                        Details = "Vehicle model has been successfully deleted by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
                return BadRequest("Failed to delete the vehicle model.");
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to delete the vehicle model.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllFuelTypes")]
        public async Task<IActionResult> GetAllFuelTypes()
        {
            try
            {
                var results = await _vehicleRepository.GetAllFuelTypeAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve fuel types.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddFuelType")]
        public async Task<IActionResult> AddFuelType([FromBody] VehicleFuelType fuelType)
        {
            try
            {
                if (fuelType == null)
                {
                    return BadRequest("Fuel type is required.");
                }

                _vehicleRepository.AddFuelType(fuelType);
                await _vehicleRepository.SaveChangesAsync();

                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Add FuelType",
                    Details = "Vehicle fuel type has been successfully added by." + User.Identity.Name,
                    Timestamp = DateTime.UtcNow
                });
                return CreatedAtAction(nameof(AddFuelType), fuelType);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add fuel type.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetFuelType/{fuelId}")]
        public async Task<IActionResult> GetFuelAsync(int fuelId)
        {
            try
            {
                var result = await _vehicleRepository.GetFuelAsync(fuelId);
                if (result == null) return NotFound("Fuel type does not exist.");

                var fuelID = result.Id;
                return Ok(result);
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve the fuel type.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditFuel/{fuelId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditFuel(int fuelId, VehicleFuelType fuelType)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetFuelAsync(fuelId);
                if (existingVehicle == null) return NotFound("The fuel type does not exist");

                existingVehicle.Id = fuelType.Id;
                existingVehicle.FuelName = fuelType.FuelName;


                if (await _vehicleRepository.SaveChangesAsync())
                {
                    return Ok(existingVehicle);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return BadRequest("Your request is invalid");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteFuel/{fuelId}")]
        public async Task<IActionResult> DeleteFuel(int fuelId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetFuelAsync(fuelId);
                if (existingVehicle == null) return NotFound("The fuel type does not exist.");

                _vehicleRepository.Delete(existingVehicle);

                if (await _vehicleRepository.SaveChangesAsync())
                {
                    return Ok(existingVehicle);
                }
                return BadRequest("Failed to delete the fuel type.");
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to delete the fuel type.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllColours")]
        public async Task<IActionResult> GetAllColours()
        {
            try
            {
                var results = await _vehicleRepository.GetAllColoursAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve colours.");
            }
        }

        [HttpPost("AddColour")]
        public async Task<IActionResult> AddColour([FromBody] Colour colour)
        {
            try
            {
                if (colour == null)
                {
                    return BadRequest("Colour is required.");
                }

                _vehicleRepository.AddColour(colour);
                await _vehicleRepository.SaveChangesAsync();

                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Add Vehicle Color",
                    Details = "Vehicle color option has been successfully added by." + User.Identity.Name,
                    Timestamp = DateTime.UtcNow
                });
                return CreatedAtAction(nameof(AddColour), colour);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add colour.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetColour/{colourId}")]
        public async Task<IActionResult> GetColourAsync(int colourId)
        {
            try
            {
                var result = await _vehicleRepository.GetColourAsync(colourId);
                if (result == null) return NotFound("Colour does not exist.");

                var colourID = result.Name;
                return Ok(result);
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve the colour.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditColour/{colourId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditColour(int colourId, Colour colour)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetColourAsync(colourId);
                if (existingVehicle == null) return NotFound("The colour does not exist");

                existingVehicle.Id = colour.Id;
                existingVehicle.Name = colour.Name;


                if (await _vehicleRepository.SaveChangesAsync())
                {
                    return Ok(existingVehicle);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return BadRequest("Your request is invalid");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteColour/{colourId}")]
        public async Task<IActionResult> DeleteColour(int colourId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetColourAsync(colourId);
                if (existingVehicle == null) return NotFound("The colour does not exist.");

                _vehicleRepository.Delete(existingVehicle);

                if (await _vehicleRepository.SaveChangesAsync())
                {
                    return Ok(existingVehicle);
                }
                return BadRequest("Failed to delete the colour.");
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to delete the colour.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllLicenseDisks")]
        public async Task<IActionResult> GetAllLicenseDisks()
        {
            try
            {
                var results = await _vehicleRepository.GetAllLicenseDiskAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve license disks.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetInsuranceCovers")]
        public async Task<IActionResult> GetInsuranceCovers()
        {
            try
            {
                var results = await _vehicleRepository.GetInsuranceCoverAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve insurance covers.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetInsurance/{insuranceId}")]
        public async Task<IActionResult> GetInsuranceAsync(int insuranceId)
        {
            try
            {
                var result = await _vehicleRepository.GetInsuranceAsync(insuranceId);
                if (result == null) return NotFound("Insurance cover does not exist.");

                var insuranceID = result.InsuranceCoverId;
                return Ok(result);
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve the insurance cover.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddInsuranceCover")]
        public async Task<IActionResult> AddInsuranceCover([FromBody] InsuranceCover insurance)
        {
            try
            {
                if (insurance == null)
                {
                    return BadRequest("Insurance Cover is required.");
                }

                _vehicleRepository.AddInsuranceCover(insurance);
                await _vehicleRepository.SaveChangesAsync();

                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Add Vehicle Insurance Cover",
                    Details = "Vehicle Insurance Cover has been successfully added by." + User.Identity.Name,
                    Timestamp = DateTime.UtcNow
                });
                return CreatedAtAction(nameof(AddInsuranceCover), insurance);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add colour.");
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditInsurance/{insuranceId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditInsurance(int insuranceId, InsuranceCover insurance)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetInsuranceAsync(insuranceId);
                if (existingVehicle == null) return NotFound("Theinsurance cover does not exist");

                existingVehicle.InsuranceCoverId = insurance.InsuranceCoverId;
                existingVehicle.InsuranceCoverName = insurance.InsuranceCoverName;


                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Edit Vehicle Insurance",
                        Details = "Vehicle Insurance options has been successfully updated by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return BadRequest("Your request is invalid");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteInsurance/{insuranceId}")]
        public async Task<IActionResult> DeleteInsurance(int insuranceId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetInsuranceAsync(insuranceId);
                if (existingVehicle == null) return NotFound("The insurance cover does not exist.");

                _vehicleRepository.Delete(existingVehicle);

                if (await _vehicleRepository.SaveChangesAsync())
                {

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Action = "Delete Vehicle Insurance",
                        Details = "Vehicle insurance has been successfully removed by." + User.Identity.Name,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingVehicle);
                }
                return BadRequest("Failed to delete the insurance cover.");
            }
            catch (Exception)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to delete the  insurance cover.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            try
            {
                var results = await _vehicleRepository.GetAllStatusAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve statuses.");
            }
        }

    }
}
