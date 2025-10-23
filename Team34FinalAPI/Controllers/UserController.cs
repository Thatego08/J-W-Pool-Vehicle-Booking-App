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

        // REMOVED ISMS_Service from constructor parameters
        public UserController(
            UserManager<User> userManager,
            IOTPService otpService,
            IOTPRepository otpRepository,
            IAuthService authService,
            IEmailService emailService,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuditLogRepository auditLogRepository,
            UserDbContext userDbContext,
            IUserRepository userRepository,
            ILogger<UserController> Logger,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipal,
            IConfiguration configuration)
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
            _emailService = emailService;
        }

        // ... rest of your controller methods
    


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

            //// Validate JAWS email domain
            //if (!model.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            //{
            //    return BadRequest(new { Message = "Only JAWS email addresses (@jaws.co.za) are allowed." });
            //}

            _logger.LogInformation("Registering user: {@Model}", model);

            try
            {
                // Check if email already exists
                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    return BadRequest(new { Message = "Email already exists." });
                }

                var user = new User
                {
                    UserName = model.Email, // Use email as username
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    //Password = model.Password,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    Role = "Driver" // Auto-assign as Driver
                };

                // Handle profile photo
                if (profilePhoto != null && profilePhoto.Length > 0)
                {
                    var filePath = Path.Combine("Images/Uploads", $"{user.Email.Replace("@", "_")}_profile.jpg");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePhoto.CopyToAsync(stream);
                    }
                    user.ProfilePhotoPath = filePath;
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //// Generate OTP for email verification
                    //var otp = await _otpService.GenerateAndSaveOtpAsync(model.Email);

                    //// Send OTP via email using Twilio
                    //try
                    //{
                    //    await _emailService.SendVerificationEmailAsync(
                    //        model.Email,
                    //        "Verify your JAWS account",
                    //        $"Your OTP for email verification is: {otp}. This OTP will expire in 10 minutes."
                    //    );
                    //}
                    //catch (Exception emailEx)
                    //{
                    //    _logger.LogError(emailEx, "Failed to send verification email");
                    //    // Continue with registration even if email fails, but log it
                    //}

                    // Create Driver role if it doesn't exist
                    if (!await _roleManager.RoleExistsAsync("Driver"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Driver"));
                    }
                    await _userManager.AddToRoleAsync(user, "Driver");

                    // Audit Log
                    await _auditLogRepository.AddLogAsync(new AuditLog
                    {
                        UserName = user.Email, // Use email instead of username
                        Action = "Register",
                        Details = $"User registered with email: {model.Email}",
                        Timestamp = DateTime.UtcNow
                    });

                    return Ok(new
                    {
                        message = "User registered successfully. Please check your email for the OTP to verify your account.",
                        email = user.Email
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
                return BadRequest(new { Message = "Registration failed. Please try again." });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                _logger.LogInformation("Login attempt for: {UserName}", model.UserName);

                // Since we're using email as username, find user by email
                // Try to find user by email first (since we use email as username)
                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user == null)
                {
                    // If not found by email, try by username
                    user = await _userManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {

                        _logger.LogWarning("User not found for email: {UserName}", model.UserName);
                        return Unauthorized(new { Message = "Invalid login attempt." });
                    }
                }
                //Temporary removal
                // Check if email is confirmed
                //if (!user.EmailConfirmed)
                //{
                //    return Unauthorized(new { Message = "Please verify your email before logging in." });
                //}

                _logger.LogInformation("User found: {Email}, attempting login with username: {UserName}",
            user.Email, user.UserName);
                // Use the email as username for login
                var result = await _authService.LoginAsync(user.Email, model.Password);

                await _auditLogRepository.AddLogAsync(new AuditLog
                {
                    UserName = user.Email, // Use email
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
        // New endpoint for email verification OTP
        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOtp([FromBody] VerifyOTPViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var otp = await _otpService.GetOtpAsync(model.Email);
            if (otp == null || otp.IsUsed || otp.ExpiryTime < DateTime.UtcNow)
                return BadRequest("Invalid or expired OTP.");

            if (!otp.Code.Trim().Equals(model.OTP.Trim()))
                return BadRequest("Incorrect OTP.");

            // Mark OTP as used
            await _otpService.MarkOtpAsUsedAsync(model.Email);

            // Find user by email and confirm their email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found.");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Email verified successfully. You can now login." });
        }

        // Updated Forgot Password to use Twilio
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found");

            // Generate OTP
            var otp = await _otpService.GenerateAndSaveOtpAsync(model.Email);

            // Send OTP via MailJet with HTML template
            await _emailService.SendPasswordResetEmailAsync(user.Email, otp);

            await _auditLogRepository.AddLogAsync(new AuditLog
            {
                UserName = user.UserName,
                Action = "Forgot Password",
                Details = "Password reset OTP sent.",
                Timestamp = DateTime.UtcNow
            });

            return Ok(new { message = "OTP has been sent to your email." });
        }
        // Updated Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found");

            // Validate OTP
            var isValidOtp = await _otpService.ValidateOtpAsync(model.Email, model.OTP);
            if (!isValidOtp)
                return BadRequest("Invalid or expired OTP.");

            // Generate a password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password
            var resetResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (resetResult.Succeeded)
            {
                await _auditLogRepository.AddLogAsync(new AuditLog
                {
                    UserName = user.UserName,
                    Action = "Reset Password",
                    Details = "Password reset successfully.",
                    Timestamp = DateTime.UtcNow
                });

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
                existingUser.Password = Pass.hashPassword(uvm.Password);
              //  existingUser.Role = uvm.Role;
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


        // 2. Generate unique username with first 3 letters of name and 2 of surname
        private async Task<string> GenerateUniqueUsername(string firstName, string lastName)
        {
            string firstPart = firstName.Length >= 3 ? firstName.Substring(0, 3) : firstName.PadRight(3, 'x');
            string lastPart = lastName.Length >= 2 ? lastName.Substring(0, 2) : lastName.PadRight(2, 'x');

            string baseUsername = (firstPart + lastPart).ToLower();
            string username = baseUsername;
            int counter = 1;

            // Check if username exists and append numbers if needed
            while (await _userManager.FindByNameAsync(username) != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;

                if (counter > 100) // Safety limit
                {
                    throw new Exception("Could not generate unique username");
                }
            }

            return username;
        }


        // 9. Email confirmation endpoint
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid confirmation link.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)

            {
                return Ok(new { message = "Email confirmed successfully. You can now login." });
            }

            return BadRequest("Error confirming email.");
        }

        // Method to send confirmation email (implement based on your email service)
        private async Task SendConfirmationEmail(string email, string confirmationLink)
        {
            // Implement your email sending logic here
            // This could use SMTP, SendGrid, etc.
            var subject = "Confirm your JAWS account";
            var body = $"Please confirm your account by clicking this link: {confirmationLink}";

            // Your email sending implementation
            await _emailService.SendEmailAsync(email, subject, body);
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
