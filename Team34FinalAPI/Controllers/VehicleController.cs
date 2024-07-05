using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;


namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<VehicleController> _logger;


        public VehicleController(IVehicleRepository vehicleRepository, IWebHostEnvironment environment, ILogger<VehicleController> logger)
        {
            _vehicleRepository = vehicleRepository;
            _environment = environment;
            _logger = logger;
        }

        [HttpGet("GetAllVehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var results = await _vehicleRepository.GetAllVehiclesAsync();
                return Ok(results);
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, "An error occured when retrieving vehicles");
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicles.");
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

        [HttpPost("AddVehicle")]
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

        /*
        [HttpPost]
        [Route("AddVehicle")]
        public async Task<IActionResult> AddVehicle(VehicleViewModel vvm)
        {
            var vehicle = new Vehicle
            {
                Name = vvm.Name,
                Description = vvm.Description,
                DateAcquired = vvm.DateAcquired,
                RegistrationNumber = vvm.RegistrationNumber,
                VehicleMakeID = vvm.VehicleMakeID,
                VehicleModelID = vvm.VehicleModelID,
                ColourID = vvm.ColourID,
                EngineNo = vvm.EngineNo,
                FuelTypeID = vvm.FuelTypeID,
                InsuranceCoverID = vvm.InsuranceCoverID,
                LicenseExpiryDate = vvm.LicenseExpiryDate,
                StatusID = vvm.StatusID,
                VIN = vvm.VIN
            };

            try
            {
                _vehicleRepository.Add(vehicle);
                await _vehicleRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(vehicle);
        } */


        [HttpPut]
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
                return CreatedAtAction(nameof(AddVehicleMake), vehicleMake);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add vehicle make.");
            }
        }

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
                return CreatedAtAction(nameof(AddVehicleModel), vehicleModel);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add vehicle model.");
            }
        }

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
                return CreatedAtAction(nameof(AddFuelType), fuelType);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add fuel type.");
            }
        }

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
                return CreatedAtAction(nameof(AddColour), colour);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add colour.");
            }
        }

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

        [HttpPost("AddInsuranceCover")]
        public async Task<IActionResult> AddInsuranceCover([FromBody] InsuranceCover insurance)
        {
            try
            {
                if (insurance == null)
                {
                    return BadRequest("Insurance cover is required.");
                }

                _vehicleRepository.AddInsuranceCover(insurance);
                await _vehicleRepository.SaveChangesAsync();
                return CreatedAtAction(nameof(AddInsuranceCover), insurance);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to add insurance cover.");
            }
        }

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

        [HttpGet("GetAllVehicleChecklist")]
        public async Task<IActionResult> GetAllVehicleChecklist()
        {
            try
            {
                var results = await _vehicleRepository.GetAllVehicleChecklistAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle checklists.");
            }
        }

        [HttpPost("VehicleChecklist")]
        public async Task<IActionResult> VehicleChecklist([FromBody] VehicleChecklist checklist)
        {
            if (checklist == null) return BadRequest("Checklist is required.");
            return Ok(new { message = "Checklist submitted successfully." });
        }
    }
}
