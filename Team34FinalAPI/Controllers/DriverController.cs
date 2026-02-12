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
//using Team34FinalAPI.Migrations.BookingDb;

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
        private readonly IAuditLogRepository _auditLogRepo;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DriverController(IDriverRepository driverRepository, IAuditLogRepository auditLogRepository,UserManager<User> userManager, ILogger<DriverController> Logger, RoleManager<IdentityRole> roleManager)
        {
            _driverRepository = driverRepository;
            _roleManager = roleManager;
            this._userManager = userManager;
            _logger = Logger;
            _auditLogRepo = auditLogRepository;
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

            var driver = new User { UserName = username, Name = dvm.Name, Surname = dvm.Surname, Email = dvm.Email,  PhoneNumber = dvm.PhoneNumber, Role = "Driver" };

            try
            {
                var result = await _userManager.CreateAsync(driver, dvm.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(driver, "Driver");
                    
                    //Audit Log stuff 
                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = username,
                        Action = "Register Driver",
                        Details = $"New driver details registered with Username: " + username,
                        Timestamp = DateTime.UtcNow
                    });

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

        [Authorize(Roles = "Admin")]   // <-- IMPORTANT: Add this attribute!
        [HttpPut]
        [Route("UpdateDriver/{userName}")]
        public async Task<IActionResult> UpdateDriver(string userName, DriverViewModel driverModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingDriver = await _userManager.FindByNameAsync(userName);
                if (existingDriver == null)
                {
                    return NotFound($"Driver with username '{userName}' does not exist.");
                }

                // Update basic fields
                existingDriver.Name = driverModel.Name;
                existingDriver.Surname = driverModel.Surname;
                existingDriver.Email = driverModel.Email;
                existingDriver.PhoneNumber = driverModel.PhoneNumber;

                // --- Role update logic ---
                var newRole = driverModel.Role;
                bool roleChanged = !string.Equals(existingDriver.Role, newRole, StringComparison.OrdinalIgnoreCase);

                if (roleChanged)
                {
                    // Ensure the role exists
                    if (!await _roleManager.RoleExistsAsync(newRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(newRole));
                    }

                    // Remove current roles, add new role
                    var currentRoles = await _userManager.GetRolesAsync(existingDriver);
                    await _userManager.RemoveFromRolesAsync(existingDriver, currentRoles);
                    await _userManager.AddToRoleAsync(existingDriver, newRole);

                    // Update custom Role property
                    existingDriver.Role = newRole;
                }

                // Save changes
                var updateResult = await _userManager.UpdateAsync(existingDriver);

                if (updateResult.Succeeded)
                {
                    // Audit log
                    string details = $"Driver details updated by {User.Identity.Name}";
                    if (roleChanged)
                        details += $". Role changed from '{existingDriver.Role}' to '{newRole}'";

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = userName,
                        Action = "Driver Details Update",
                        Details = details,
                        Timestamp = DateTime.UtcNow
                    });

                    return Ok(existingDriver);
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the driver with username '{UserName}'", userName);
                return StatusCode(500, "Internal server error. Please contact support.");
            }
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
                {
                    //Audit Log stuff 
                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = userName,
                        Action = "Delete Driver",
                        Details = $"Driver details removed effectively by : " + userName,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingDriver);
                }

            }

            catch (Exception)
            {
                return StatusCode(500, "Internal server error.Please contact support");

            }
            return BadRequest("Your request is invalid");
        }
    }
}