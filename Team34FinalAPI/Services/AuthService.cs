using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string username, string password, string role, User user);
        Task<string> LoginAsync(string username, string password);
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

        public async Task<SignInResult> SignIn(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);
            return result;
        }

        public async Task<string> RegisterAsync(string username, string password, string role, User Uvm)
        {
            //try
            {
                var user = new User
                {
                    UserName = Uvm.UserName,
                    Name = Uvm.Name,
                    Surname = Uvm.Surname,
                    PhoneNumber = Uvm.PhoneNumber,
                    Email = Uvm.Email,
                    Password = Uvm.Password
                    // Include other necessary properties
                };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);

                    return await GenerateJwtToken(user); // Await here
                }

            }
            //catch
            //(Exception ex)
            {
                throw new Exception("Registration failed");
            }



        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new Exception("Invalid login attempt");
            }

            return await GenerateJwtToken(user); // Await here
        }
    }
}
