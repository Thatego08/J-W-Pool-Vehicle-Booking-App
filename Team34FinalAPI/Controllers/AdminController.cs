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
        public AdminController(IAdminRepo adminRepo, IOTPService otpService, IConfiguration configuration, OTPSettingsService otpSettingsService, UserManager<User> userManager, ILogger<AdminController> logger)
        {
            _adminRepo = adminRepo;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _otpService = otpService;
            _otpSettingsService = otpSettingsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var results = await _adminRepo.GetAllAdminsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
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

                if (await _adminRepo.SaveChangesAync())
                {
                    return Ok(existingAdmin);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
            return BadRequest("Your request is invalid");
        }


        [Authorize(Roles = "Admin")]

        [HttpGet]
        [Route("SearchAdmin/{userName}")]
        public async Task<IActionResult> GetAdminAsync(string userName)
        {
            try
            {
                var results = await _adminRepo.GetAdminAsync(userName);
                if (results == null) return NotFound("Admin does not exist. Enter a valid admin");
                return Ok(results);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please contact support");
            }
            return BadRequest("Your request is invalid");
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
                    return Ok(existingAdmin);
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



