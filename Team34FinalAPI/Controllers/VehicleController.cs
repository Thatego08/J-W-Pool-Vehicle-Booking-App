using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.ViewModels;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        [Route("GetAllVehicles")] // Return list of vehicles
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var results = await _vehicleRepository.GetAllVehiclesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("GetVehicle/{vehicleId}")] // Corrected route template
        public async Task<IActionResult> GetVehicleAsync(int vehicleId)
        {
            try
            {
                var results = await _vehicleRepository.GetVehicleAsync(vehicleId);

                if (results == null) return NotFound("Vehicle does not exist");
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("AddVehicle")]
        public async Task<IActionResult> AddVehicle(VehicleViewModel vvm)
        {
            var vehicle = new Vehicle
            {
                Name = vvm.Name,
                Image = vvm.Image,
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
        }

        [HttpPut]
        [Route("EditVehicle/{vehicleId}")] // Corrected route template
        public async Task<ActionResult<VehicleViewModel>> EditVehicle(int vehicleId, VehicleViewModel vehicleModel)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetVehicleAsync(vehicleId);
                if (existingVehicle == null) return NotFound("The vehicle does not exist");

                existingVehicle.Name = vehicleModel.Name;
                existingVehicle.Image = vehicleModel.Image;
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

        [HttpDelete]
        [Route("DeleteVehicle/{vehicleId}")] // Corrected route template
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetVehicleAsync(vehicleId);
                if (existingVehicle == null) return NotFound("The vehicle does not exist");

                _vehicleRepository.Delete(existingVehicle);

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
    }
}

