using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Factory
{
    public class UserClaimsFactory
    {
        public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
        {
            public AppUserClaimsPrincipalFactory(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
            {
            }

        }
    }
}
