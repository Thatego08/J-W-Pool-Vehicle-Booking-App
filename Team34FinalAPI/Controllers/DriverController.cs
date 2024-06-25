using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata.Ecma335;
using Team34FinalAPI.Models;
using Microsoft.Identity.Client;
using Team34FinalAPI.ViewModels;
using System.Linq.Expressions;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;

        public DriverController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        [HttpGet]
        [Route("GetAllDrivers")] //Displays All The Drivers Stored On The Database

        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var results = await _driverRepository.GetAllDriverAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please Contact Support");
            }

        }

        [HttpGet]
        [Route("SearchDriver/{userName}")]
        public async Task<IActionResult> GetDriverAsync(string userName)
        {
            try
            {
                var results = await _driverRepository.GetDriverAsync(userName);
                if (results == null) return NotFound("Driver does not exist. Enter a valid driver");
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
        }

        [HttpPost]
        [Route("RegisterDriver")]
        public async Task<IActionResult> RegisterDriver(DriverViewModel dvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var driver = new Driver { UserName = dvm.UserName, Name = dvm.Name, Surname = dvm.Surname, Email = dvm.Email, Password = dvm.Password, PhoneNumber = dvm.PhoneNumber };

            try
            {
                _driverRepository.Add(driver);
                await _driverRepository.SaveChangesAync();

                return Ok(driver);
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                Console.WriteLine(ex.Message);
                return BadRequest("Failed to register driver. Please retry.");
            }
        }

        [HttpPut]
        [Route("UpdateDriver/{userName}")]
        public async Task<ActionResult<DriverViewModel>> UpdateDriver(string userName, DriverViewModel driverModel)
        {
            try
            {
                var existingDriver = await _driverRepository.GetDriverAsync(userName);
                if (existingDriver == null) return NotFound($"The driver does not exists");
                existingDriver.Name = driverModel.Name;
                existingDriver.Surname = driverModel.Surname;
                existingDriver.UserName = driverModel.UserName;
                existingDriver.Email = driverModel.Email;
                existingDriver.PhoneNumber = driverModel.PhoneNumber;

                if (await _driverRepository.SaveChangesAync())
                {
                    return Ok(existingDriver);
                }

            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
            return BadRequest("Your request is invalid ");
        }

        [HttpDelete]
        [Route("DeleteDriver/{userName}")]

        public async Task <IActionResult> DeleteDriver(string userName)
        {
            try
            {
                var existingDriver = await _driverRepository.GetDriverAsync(userName);
                if (existingDriver == null) return NotFound($"The driver does not exist");
                _driverRepository.Delete(existingDriver);

                if (await _driverRepository.SaveChangesAync())
                    return Ok(existingDriver);
               
            }
            
              catch(Exception)
            {
                return StatusCode(500, "Internal server error.Please contact support");

            }
            return BadRequest("Your request is invalid");
        }
    }
}