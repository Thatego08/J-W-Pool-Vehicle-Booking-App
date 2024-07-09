using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata.Ecma335;
using Team34FinalAPI.Models;
using Microsoft.Identity.Client;
using Team34FinalAPI.ViewModels;


using Team34FinalAPI.Tools;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Team34FinalAPI.Controllers
{

    //Comment to disable locking
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;

        private readonly ILogger<DriverController> _logger;
        private readonly UserManager<User> _userManager;

        public DriverController(IDriverRepository driverRepository, UserManager<User> userManager, ILogger<DriverController> Logger)
        {
            _driverRepository = driverRepository;
            this._userManager = userManager;
            _logger = Logger;
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Driver,Admin")]

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


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterDriver")]
        public async Task<IActionResult> RegisterDriver(DriverViewModel dvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Registering user: {@Model}", dvm);

            string username = GenerateUsername(dvm.Name, dvm.Surname);

            var driver = new User { UserName = username, Name = dvm.Name, Surname = dvm.Surname, Email = dvm.Email, Password = Pass.hashPassword(dvm.Password), PhoneNumber = dvm.PhoneNumber, Role = "Driver" };

            try
            {
                var result = await _userManager.CreateAsync(driver, dvm.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(driver, "Driver");

                    return Ok("User registered successfully" + "Your Username is: " + username);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogInformation("User creation error: {Error}", error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
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
        //Username Function
        private string GenerateUsername(string firstName, string lastName)
        {
            string firstPart = firstName.Length >= 4 ? firstName.Substring(0, 4) : firstName;
            string lastPart = lastName.Length >= 2 ? lastName.Substring(0, 2) : lastName;
            return firstPart + lastPart;
        }

        [Authorize(Roles = "Driver, Admin")]

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
                //existingDriver.UserName = driverModel.UserName;
                existingDriver.Email = driverModel.Email;
                existingDriver.PhoneNumber = driverModel.PhoneNumber;

                if (await _driverRepository.SaveChangesAync())
                {
                    return Ok(existingDriver);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
            return BadRequest("Your request is invalid ");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("DeleteDriver/{userName}")]

        public async Task<IActionResult> DeleteDriver(string userName)
        {
            try
            {
                var existingDriver = await _driverRepository.GetDriverAsync(userName);
                if (existingDriver == null) return NotFound($"The driver does not exist");
                _driverRepository.Delete(existingDriver);

                if (await _driverRepository.SaveChangesAync())
                    return Ok(existingDriver);

            }

            catch (Exception)
            {
                return StatusCode(500, "Internal server error.Please contact support");

            }
            return BadRequest("Your request is invalid");
        }
    }
}