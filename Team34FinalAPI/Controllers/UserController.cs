using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using Team34FinalAPI.Models;
using Team34FinalAPI.Services;
using Team34FinalAPI.Tools;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private IAuditLogRepository _auditLogRepository;
        private readonly IOTPService _otpService;
        private readonly IOTPRepository _otpRepository;

        public UserController(UserManager<User> userManager,IOTPService otpService,IOTPRepository otpRepository, IAuthService authService, IEmailService emailService,  SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IAuditLogRepository auditLogRepository, UserDbContext userDbContext,  IUserRepository userRepository, ILogger<UserController> Logger, IUserClaimsPrincipalFactory<User> userClaimsPrincipal, IConfiguration configuration)
        {
            this._userDbContext = userDbContext;
            _userRepository = userRepository;
            _logger = Logger;
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _claimsPrincipalFactory = userClaimsPrincipal;
            _auditLogRepository = auditLogRepository;
            _otpService = otpService;
            _otpRepository = otpRepository;

            //_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailService = emailService;

        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var results = await _userRepository.GetAllUsersAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException, "Inner exception details.");
                }
                return StatusCode(500, "Internal Server error, contact support");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Registering user: {@Model}", model);

            string username = GenerateUsername(model.Name, model.Surname);

            try
            {
                var user = new User
                {
                    UserName = username,
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Password = Pass.hashPassword(model.Password),
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    Role = model.Role
                };

                var existingUser = await _userManager.FindByNameAsync(user.UserName);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "Username already exists." });
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //Audit Log Stuff
                    await _auditLogRepository.AddLogAsync(new AuditLog
                    {
                        UserName = username,
                        Action = "Register",
                        Details = $"User registered with username: "+ username,
                        Timestamp = DateTime.UtcNow
                    });

                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }
                    await _userManager.AddToRoleAsync(user, model.Role);

                    
                    return Ok(new { message = "User registered successfully", username = user.UserName });
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
                // Check if the error is due to duplicate username
                if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    return BadRequest(new { message = "Username already exists." });
                }
                return BadRequest(new { message = "Registration failed." });
            }
            catch (DbUpdateException dbEx)
            {
                var sqlException = dbEx.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    _logger.LogError(sqlException, "SQL Error Number: {ErrorNumber}, Message: {ErrorMessage}", sqlException.Number, sqlException.Message);
                    return StatusCode(500, $"A database error occurred: {sqlException.Message}");
                }

                _logger.LogError(dbEx, "A database error occurred while registering the user.");
                return StatusCode(500, "A database error occurred. Please check the details.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException, "Inner exception details.");
                }
                return BadRequest(new { Message = ex.Message });
            }
        }

        private string GenerateUsername(string firstName, string lastName)
        {
            string firstPart = firstName.Length >= 4 ? firstName.Substring(0, 4) : firstName;
            string lastPart = lastName.Length >= 2 ? lastName.Substring(0, 2) : lastName;
            return firstPart + lastPart;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var result = await _authService.LoginAsync(model.UserName, model.Password);
                await _auditLogRepository.AddLogAsync(new AuditLog
                {
                    UserName = model.UserName,
                    Action = "Login",
                    Details = "User logged in.",
                    Timestamp = DateTime.UtcNow
                });
                return Ok(new { Token = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");

                return Unauthorized(new { Message = ex.Message });
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                await _auditLogRepository.AddLogAsync(new AuditLog
                {
                    UserName = User.Identity.Name,
                    Action = "Logout",
                    Details = "User logged out successfully.",
                    Timestamp = DateTime.UtcNow
                });
                return Ok(new { Message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(500, "Internal server error. Please contact support.");
            }
        }
        // In UserController.cs
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // Get the username of the logged-in user
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(new { message = "User not found." });
            }

            // Retrieve the user profile based on the username
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Return the user profile information
            var userProfile = new
            {
                user.UserName,
                user.Email,
                user.Name,
                user.Surname,
                user.Role
            };

            return Ok(userProfile);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Edit-Admin-User/{userName}")]
        public async Task<IActionResult> EditUser(string userName, UserViewModel uvm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _userRepository.GetUserAsync(userName);
                if (existingUser == null) return NotFound("This user does not exist");

                existingUser.Name = uvm.Name;
                existingUser.Surname = uvm.Surname;
                existingUser.PhoneNumber = uvm.PhoneNumber;
                existingUser.Email = uvm.Email;
                existingUser.Password = Pass.hashPassword(uvm.Password);
                existingUser.Role = uvm.Role;
                existingUser.EmailConfirmed = false;
                existingUser.PhoneNumberConfirmed = false;
                existingUser.TwoFactorEnabled = false;
                existingUser.LockoutEnabled = false;
                existingUser.AccessFailedCount = 0;

                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                if (currentRoles.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        return BadRequest($"Failed to remove user from current roles: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");
                    }
                }

                if (!await _roleManager.RoleExistsAsync(uvm.Role))
                {
                    var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(uvm.Role));
                    if (!createRoleResult.Succeeded)
                    {
                        return BadRequest($"Failed to create role '{uvm.Role}': {string.Join(", ", createRoleResult.Errors.Select(e => e.Description))}");
                    }
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(existingUser, uvm.Role);
                if (!addToRoleResult.Succeeded)
                {
                    return BadRequest($"Failed to assign user to role '{uvm.Role}': {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                }

                var saveResult = await _userManager.UpdateAsync(existingUser);

                if (saveResult.Succeeded)
                {
                    return Ok(existingUser);
                }
                else
                {
                    return BadRequest("Your request is invalid.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the user.");
                return StatusCode(500, "Internal server error. Please contact support. Details: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPut]
        [Route("Edit-Driver-User/{userName}")]
        public async Task<IActionResult> EditDriver(string userName, EditViewModel uvm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _userRepository.GetUserAsync(userName);
                if (existingUser == null) return NotFound("This user does not exist");

                existingUser.Name = uvm.Name;
                existingUser.Surname = uvm.Surname;
                existingUser.PhoneNumber = uvm.PhoneNumber;
                existingUser.Email = uvm.Email;
                existingUser.Password = Pass.hashPassword(uvm.Password);
                existingUser.EmailConfirmed = true;
                existingUser.PhoneNumberConfirmed = true;
                existingUser.TwoFactorEnabled = true;
                existingUser.LockoutEnabled = true;
                existingUser.AccessFailedCount = 0;

                var saveResult = await _userManager.UpdateAsync(existingUser);

                if (saveResult.Succeeded)
                {
                    return Ok(existingUser);
                }
                else
                {
                    return BadRequest("Your request is invalid.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the user.");
                return StatusCode(500, "Internal server error. Please contact support. Details: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for forgot password request.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Processing forgot password request for email: {Email}", model.Email);

            var user = await _userRepository.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", model.Email);
                return NotFound("User not found");
            }

            // Generate and save OTP
            var otp = await _otpService.GenerateAndSaveOtpAsync(model.Email);

            // Send OTP via email
            await _emailService.SendEmailAsync(user.Email, "Your OTP", $"Your OTP for password reset is: {otp}");

            _logger.LogInformation("OTP sent to email: {Email}", user.Email);
            //return Ok("OTP has been sent to your email.");
            return Ok(new { message = "OTP has been sent" });

        }

        [HttpPost]
        [Route("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOTPViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in verify-otp request.");
                return BadRequest(ModelState);
            }

            var otp = await _otpRepository.GetOtpAsync(model.Email);
            if (otp == null)
            {
                _logger.LogWarning("OTP not found or already used for email: {Email}", model.Email);
                return BadRequest("Invalid OTP or OTP has expired.");
            }

            _logger.LogInformation("Retrieved OTP: {StoredOtp}, Entered OTP: {EnteredOtp}", otp.Code, model.OTP);

            if (otp.Code.ToUpper() != model.OTP.ToUpper())
            {
                _logger.LogWarning("OTP does not match for email: {Email}", model.Email);
                return BadRequest("Invalid OTP.");
            }

            if (otp.ExpiryTime < DateTime.UtcNow)
            {
                _logger.LogWarning("OTP expired for email: {Email}", model.Email);
                return BadRequest("OTP has expired.");
            }

            // Mark the OTP as used
            await _otpRepository.MarkOtpAsUsedAsync(model.Email);

            _logger.LogInformation("OTP verified successfully for email: {Email}", model.Email);
            return Ok(new { message = "OTP verified successfully." });
        }





        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found");

            // Validate OTP
            var otp = await _otpService.GetOtpAsync(model.Email);
            if (otp == null)
                return BadRequest("Invalid OTP.");

            if (otp.Code != model.OTP)
                return BadRequest("Incorrect OTP.");

            if (otp.ExpiryTime < DateTime.UtcNow)
                return BadRequest("OTP has expired.");

            if (otp.IsUsed)
                return BadRequest("OTP has already been used.");

            // If OTP is valid, mark it as used
            await _otpService.MarkOtpAsUsedAsync(model.Email);

            // Proceed with password reset
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
                return Ok("Password has been reset.");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }


    }
}
