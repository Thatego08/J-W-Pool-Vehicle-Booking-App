using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Team34FinalAPI.Controllers;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(User user, string password);
        Task<string> LoginAsync(string email, string password);

        // In your IAuthService interface, add:
        Task<string> RegisterSimpleAsync(SimpleRegisterModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


// In your AuthService class, implement:
public async Task<string> RegisterSimpleAsync(SimpleRegisterModel model)
{
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
        LockoutEnabled = true,
        AccessFailedCount = 0,
        Role = "Driver"
    };

    var result = await _userManager.CreateAsync(user, model.Password);
    if (result.Succeeded)
    {
        //if (!await _roleManager.RoleExistsAsync("Driver"))
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Driver"));
        //}
        await _userManager.AddToRoleAsync(user, "Driver");
        return await GenerateJwtToken(user);
    }
    else
    {
        throw new Exception($"Registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }
}
        public async Task<string> RegisterAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Auto-assign Driver role
                await _userManager.AddToRoleAsync(user, "Driver");
                return await GenerateJwtToken(user);
            }
            else
            {
                throw new Exception($"Registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            // Find user by email (which is also the username)
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid login attempt");
            }

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid login attempt");
            }

            return await GenerateJwtToken(user);
        }
    }
}