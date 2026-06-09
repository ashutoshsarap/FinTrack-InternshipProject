using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class UpdateUserSubscriptionService : IUpdateUserSubscriptionPlan
    {

        private UserManager<ApplicationUser> _userManager;

        public UpdateUserSubscriptionService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DowngradeSubscriptionPlan(ApplicationUser applicationUser)
        {
            if (applicationUser != null)
            {
                applicationUser.SubscriptionPlan = SubscriptionPlan.Free;
                await _userManager.UpdateAsync(applicationUser);
            }
        }

        public async Task UpgradeSubscriptionPlan(ApplicationUser applicationUser)
        {
            if (applicationUser != null)
            {
                applicationUser.SubscriptionPlan = SubscriptionPlan.Premium;
                await _userManager.UpdateAsync(applicationUser);
            }

        }
    }
}
