using Microsoft.AspNetCore.Identity;

namespace Team34FinalAPI.Models
{
    public class IdentityUserRole
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "Driver" };

            foreach (var roleName in roleNames)
            {
                bool roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    // Create the role if it doesn't exist
                    var role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
        }
        // Navigation property to users

        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();
        //public required ICollection<AppUserRoles> UserRoles { get; set; } = new List<AppUserRoles>();
    }
}
