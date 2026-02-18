using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using Team34FinalAPI.Models;
using Team34FinalAPI.Services;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // Initialize Repo 
        private readonly IAdminRepo _adminRepo;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly OTPSettingsService _otpSettingsService;
        private readonly IOTPService _otpService;
        private readonly IAuditLogRepository _auditLogRepo;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IAdminRepo adminRepo, IOTPService otpService,IAuditLogRepository auditLogRepository, IConfiguration configuration, OTPSettingsService otpSettingsService, UserManager<User> userManager, ILogger<AdminController> logger, RoleManager<IdentityRole> roleManager)
        {
            _adminRepo = adminRepo;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
            _otpService = otpService;
            _otpSettingsService = otpSettingsService;
            _auditLogRepo = auditLogRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                // Optional: project to a DTO that shows the role as "Admin"
                var result = admins.Select(u => new
                {
                    u.UserName,
                    u.Name,
                    u.Surname,
                    u.Email,
                    u.PhoneNumber,
                    Role = "Admin"  // Hardcode or fetch actual roles if needed
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching admins");
                return StatusCode(500, "Internal Server Error. Please Contact Support");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("UpdateAdmin/{userName}")]
        public async Task<ActionResult<AdminViewModel>> UpdateAdmin(string userName, AdminViewModel adminModel)
        {
            try
            {
                var existingAdmin = await _adminRepo.GetAdminAsync(userName);
                if (existingAdmin == null) return NotFound($"The admin does not exist");

                existingAdmin.Name = adminModel.Name;
                existingAdmin.Surname = adminModel.Surname;
                existingAdmin.Email = adminModel.Email;
                existingAdmin.PhoneNumber = adminModel.PhoneNumber;

                //Role Update
                var newRole = adminModel.Role;

                bool roleChanged = !string.Equals(existingAdmin.Role, newRole, StringComparison.OrdinalIgnoreCase);

                if (roleChanged)
                {
                    // Ensure the new role exists
                    if (!await _roleManager.RoleExistsAsync(newRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(newRole));
                    }

                    // Remove from current role(s) – assuming single role
                    var currentRoles = await _userManager.GetRolesAsync(existingAdmin);
                    await _userManager.RemoveFromRolesAsync(existingAdmin, currentRoles);

                    // Add to new role
                    await _userManager.AddToRoleAsync(existingAdmin, newRole);

                    // Update the custom Role property
                    existingAdmin.Role = newRole;
                }

                // Save all changes via UserManager (includes Role property now)
                var updateResult = await _userManager.UpdateAsync(existingAdmin);

                if (updateResult.Succeeded)
                {
                    // Audit log – include role change details if applicable
                    string details = $"Admin details updated by {User.Identity.Name}";
                    if (roleChanged)
                        details += $". Role changed from '{existingAdmin.Role}' to '{newRole}'";

                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = userName,
                        Action = "Update admin details",
                        Details = details,
                        Timestamp = DateTime.UtcNow
                    });

                    return Ok(existingAdmin);
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
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search/{userName}")]
        public async Task<IActionResult> SearchAdmin(string userName)
        {
            try
            {
                var admin = await _adminRepo.GetAdminAsync(userName);
                if (admin == null)
                    return NotFound("Admin does not exist. Enter a valid admin.");
                return Ok(admin);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support.");
            }
        }


        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(AdminViewModel avm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string username = GenerateUsername(avm.Name, avm.Surname);

            var admin = new User
            {
                UserName = username,
                Name = avm.Name,
                Surname = avm.Surname,
                Email = avm.Email,
                PhoneNumber = avm.PhoneNumber,
                Role = "Admin"
            };

            try
            {

                var result = await _userManager.CreateAsync(admin, avm.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");

                    //Audit Log stuff 
                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = username,
                        Action = "Register admin details",
                        Details = $"Admin registration completed by: " + username,
                        Timestamp = DateTime.UtcNow
                    });


                    return Ok("User registered successfully. Your Username is: " + username);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error registering admin: {Error}", ex.Message);
                return BadRequest("Failed to register admin. Please retry.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userName}")]          //  RESTful route
        public async Task<IActionResult> GetAdminByUserName(string userName)
        {
            try
            {
                var admin = await _adminRepo.GetAdminAsync(userName);
                if (admin == null)
                    return NotFound($"Admin '{userName}' not found.");

                // Ensure the Role property is populated (should be, if your repo returns the full User)
                return Ok(admin);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]

        [Route("DeleteAdmin/{userName}")]
        public async Task<IActionResult> DeleteAdmin(string userName)
        {
            try
            {
                var existingAdmin = await _adminRepo.GetAdminAsync(userName);
                if (existingAdmin == null) return NotFound($"The admin does not exist");
                _adminRepo.Delete(existingAdmin);

                if (await _adminRepo.SaveChangesAync())
                {
                    //Audit Log stuff 
                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                        UserName = userName,
                        Action = "Delete admin details",
                        Details = $"Admin details have been deleted by: " + userName,
                        Timestamp = DateTime.UtcNow
                    });


                    return Ok(existingAdmin);
           
                }
                     }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
            return BadRequest("Your request is invalid");

        }




        private string GenerateUsername(string firstName, string lastName)
        {
            string firstPart = firstName.Length >= 4 ? firstName.Substring(0, 4) : firstName;
            string lastPart = lastName.Length >= 2 ? lastName.Substring(0, 2) : lastName;
            return firstPart + lastPart;
        }

        [HttpGet]
        [Route("otp-expiration")]
        public async Task<IActionResult> GetOtpExpirationTime()
        {
            var expirationTime = await _otpSettingsService.GetOtpExpirationTimeAsync();
            return Ok(new { expirationTime });
        }

        [HttpPost]
        [Route("update-otp-expiration")]
        public async Task<IActionResult> UpdateOtpExpirationTime([FromBody] int newExpirationTime)
        {
            await _otpSettingsService.UpdateOtpExpirationTimeAsync(newExpirationTime);
            return Ok();
        }


    }
}



