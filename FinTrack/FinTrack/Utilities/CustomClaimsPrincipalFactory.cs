using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FinTrack.Utilities
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if (user.SubscriptionPlan != SubscriptionPlan.Free)
            {
                Claim subscriptionClaim = new Claim("SubscriptionPlan", user.SubscriptionPlan.ToString());
                ((ClaimsIdentity)principal.Identity).AddClaim(subscriptionClaim);
            }
            return principal;
        }
    }
}
