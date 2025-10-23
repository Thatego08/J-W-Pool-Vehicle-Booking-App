using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Team34FinalAPI.Models;
using Team34FinalAPI.Services;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register-simple")]
        public async Task<IActionResult> RegisterSimple([FromBody] SimpleRegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "Email already exists." });
                }

                var token = await _authService.RegisterSimpleAsync(model);

                return Ok(new
                {
                    Message = "Registration successful!",
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("register-simpler")]
        public async Task<IActionResult> RegisterSimpler([FromBody] SimpleRegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "Email already exists." });
                }

                // Create a proper User entity with all required fields
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true, // Identity usually expects this to be true
                    AccessFailedCount = 0,
                    Role = "Driver"
                };

                // Use UserManager directly instead of AuthService.RegisterAsync
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create Driver role if it doesn't exist
                    if (!await _roleManager.RoleExistsAsync("Driver"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Driver"));
                    }
                    await _userManager.AddToRoleAsync(user, "Driver");

                    // Generate token
                    var token = await _authService.RegisterSimpleAsync(model);

                    return Ok(new
                    {
                        Message = "Registration successful!",
                        Token = token,
                        User = new
                        {
                            user.Email,
                            user.Name,
                            user.Surname,
                            user.Role
                        }
                    });
                }
                else
                {
                    // Return the actual errors from Identity
                    return BadRequest(new
                    {
                        Message = "Registration failed.",
                        Errors = result.Errors.Select(e => e.Description)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                return BadRequest(new { Message = "Registration failed. Please try again." });
            }
        }

        [HttpPost("login-simple")]
        public async Task<IActionResult> LoginSimple([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Use AuthService.LoginAsync which handles the authentication
                var token = await _authService.LoginAsync(model.UserName, model.Password);

                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        // You can get user details from the token or query the user again
                        Email = model.UserName
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }

    // SimpleRegisterModel class
    public class SimpleRegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
