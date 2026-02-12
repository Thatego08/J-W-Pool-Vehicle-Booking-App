using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Team34FinalAPI.Models;
using Team34FinalAPI.Services;
using Team34FinalAPI.Tools;
using Team34FinalAPI.ViewModels;
using System.Text;

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
        private readonly ISMS_Service _smsService;

        public UserController(UserManager<User> userManager,ISMS_Service smsService,IOTPService otpService,IOTPRepository otpRepository, IAuthService authService, IEmailService emailService,  SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IAuditLogRepository auditLogRepository, UserDbContext userDbContext,  IUserRepository userRepository, ILogger<UserController> Logger, IUserClaimsPrincipalFactory<User> userClaimsPrincipal, IConfiguration configuration)
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
            _smsService = smsService;
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
        public async Task<IActionResult> Register([FromForm] UserViewModel model, IFormFile profilePhoto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Registering user: {@Model}", model);

            try
            {
                var userRole = string.IsNullOrWhiteSpace(model.Role) ? "Driver" : model.Role;
                var user = new User
                {
                    UserName = model.Email, // Use email as username
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    Role = userRole
                };

                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "Email already exists." });
                }

                // Handle the profile photo
                if (profilePhoto != null && profilePhoto.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, $"{user.UserName}_profile.jpg");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePhoto.CopyToAsync(stream);
                    }
                    user.ProfilePhotoPath = Path.Combine("Images", "Uploads", $"{user.UserName}_profile.jpg");
                }

                // Create user with password - Identity will handle hashing
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Rest of your success code remains the same...
                    await _auditLogRepository.AddLogAsync(new AuditLog
                    {
                        UserName = user.UserName,
                        Action = "Register",
                        Details = $"User registered with email: " + user.Email,
                        Timestamp = DateTime.UtcNow
                    });



                    if (!await _roleManager.RoleExistsAsync(userRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }
                    await _userManager.AddToRoleAsync(user, userRole);

                    // Generate JWT token
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(3),
                        signingCredentials: creds
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new
                    {
                        message = "User registered successfully",
                        email = user.Email,
                        token = tokenString
                    });
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                return BadRequest(new { Message = ex.Message });
            }
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

            // Build the URL for the profile photo
            var profilePhotoUrl = string.Empty;
            if (!string.IsNullOrEmpty(user.ProfilePhotoPath))
            {
                profilePhotoUrl = $"{Request.Scheme}://{Request.Host}/Images/Uploads/{Path.GetFileName(user.ProfilePhotoPath)}";
            }

            // Return the user profile information
            var userProfile = new
            {
                user.UserName,
                user.Email,
                user.Name,
                user.Surname,
                user.Role,

                ProfilePhotoUrl = profilePhotoUrl
            };

            await _auditLogRepository.AddLogAsync(new AuditLog
            {
                UserName = User.Identity.Name,
                Action = "Profile View",
                Details = "User viewed their profile successfully.",
                Timestamp = DateTime.UtcNow
            });

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
                //existingUser.Password = Pass.hashPassword(uvm.Password);
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
               // existingUser.Password = Pass.hashPassword(uvm.Password);
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

        [HttpPut]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUser(string userName ,[FromBody] UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the current user based on their email (or ID if available)
            var user = await _userRepository.GetUserAsync(userName);
            if (user == null)
                return NotFound("User not found");

            // Update basic profile details
  
            // Handle password change if both CurrentPassword and NewPassword are provided
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                // Verify the current password is correct
                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheck)
                    return BadRequest("Incorrect current password");

                // Check if the new password is the same as the current password
                var passwordHasher = new PasswordHasher<User>();
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.NewPassword) == PasswordVerificationResult.Success)
                    return BadRequest(new { message = "The new password cannot be the same as the current password." });


                // Change the password
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }

            // Update user in the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _auditLogRepository.AddLogAsync(new AuditLog
            {
                UserName = User.Identity.Name,
                Action = "Update Password",
                Details = "User has successfully updated their password.",
                Timestamp = DateTime.UtcNow
            });

            return Ok(new { message = "Profile updated successfully." });
        }


        [HttpPut]
        [Route("update-details")]
        public async Task<IActionResult> UpdateDetails(string userName, [FromForm] UpdateDetailsViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserAsync(userName);
            if (user == null)
                return NotFound("User not found");

            // Update basic profile details
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;

            // Handle profile picture update
            if (model.ProfilePhoto != null)
            {
                // Check if a new profile photo is uploaded
                if (model.ProfilePhoto != null)
                {
                    // Save the uploaded file
                    var filePath = Path.Combine("Images/Uploads", model.ProfilePhoto.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePhoto.CopyToAsync(stream);
                    }

                    // Update the user's profile photo path in the user object
                    user.ProfilePhotoPath = model.ProfilePhoto.FileName; // Assuming you have a property for this
                }
            }
            
           
            // Update user in the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _auditLogRepository.AddLogAsync(new AuditLog
            {
                UserName = User.Identity.Name,
                Action = "Update Details",
                Details = "User has successfully updated their details.",
                Timestamp = DateTime.UtcNow
            });
            return Ok(new { message = "Profile updated successfully." });
        }

        private async Task<string> SaveProfilePhoto(IFormFile file, string userName)
        {
            // Logic to save the profile photo and return the URL
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Images/Uploads");
            var filePath = Path.Combine(uploads, $"{userName}_profile.jpg");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/Images/Uploads/{userName}_profile.jpg"; // Return the relative URL
        }



        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.FindByEmailAsync(model.Email); // Fetch user by email
            if (user == null)
                return NotFound("User not found");

            // Generate OTP
            var otp = new OTP
            {
                Email = model.Email,
                Code = new Random().Next(100000, 999999).ToString(), // Generate 6-digit OTP
                ExpiryTime = DateTime.UtcNow.AddMinutes(10), // OTP valid for 10 minutes
                IsUsed = false
            };

            // Save OTP in the OTP table
            await _otpRepository.SaveOtpAsync(otp);

            // Send OTP via email
            await _emailService.SendEmailAsync(user.Email, "Your OTP", $"Your OTP for password reset is: {otp.Code}");

            //  await _smsService.SendSmsAsync(user.PhoneNumber, $"Your OTP for password reset is: {otp.Code}");

            await _auditLogRepository.AddLogAsync(new AuditLog
            {
                UserName = user.Email,
                Action = "Forgot Password",
                Details = "User has successfully reset their password.",
                Timestamp = DateTime.UtcNow
            });
            return Ok(new { message = "OTP has been sent to your email." });
        }

       
        [HttpPost]
        [Route("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOTPViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retrieve OTP by email
            var otp = await _otpRepository.GetOtpAsync(model.Email);
            if (otp == null || otp.IsUsed || otp.ExpiryTime < DateTime.UtcNow)
                return BadRequest("Invalid or expired OTP.");
            
            _logger.LogInformation($"Stored OTP: {otp.Code}, Entered OTP: {model.OTP}");


            // Ensure case-insensitive comparison
            if (!otp.Code.Trim().ToUpper().Equals(model.OTP.Trim().ToUpper()))
                return BadRequest("Incorrect OTP.");

            return Ok(new { message = "OTP verified successfully." });
        }





        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found");

            // Validate OTP (use your existing logic)
            var otp = await _otpRepository.GetOtpAsync(model.Email);
            if (otp == null || otp.Code != model.OTP || otp.ExpiryTime < DateTime.UtcNow || otp.IsUsed)
                return BadRequest("Invalid or expired OTP.");

            // Mark OTP as used
            await _otpRepository.MarkOtpAsUsedAsync(model.Email);

            // Generate a password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the user's password using UserManager's ResetPasswordAsync
            var resetResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (resetResult.Succeeded)
            {
                return Ok(new { message = "Password has been reset successfully." });
            }
            else
            {
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }


    [HttpGet]
    public async Task<IActionResult> GetAuditLogs([FromQuery] string username)
    {
        var logs = await _auditLogRepository.GetAuditLogsAsync(username);
        return Ok(logs);
    }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditAuditLogDetails(int id, [FromBody] EditAuditLogRequest request)
        {
            if (string.IsNullOrEmpty(request.NewDetails))
            {
                return BadRequest("Details cannot be empty.");
            }

            await _auditLogRepository.EditAuditLogDetailsAsync(id, request.NewDetails);
            return Ok(new { message = "Audit log details updated successfully." });
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAuditLog(int id)
        {
            await _auditLogRepository.DeleteAuditLogAsync(id);
            return Ok(new { message = "Audit log deleted successfully." });
        }



    }



}
