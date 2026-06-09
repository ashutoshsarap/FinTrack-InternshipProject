using FinTrack.Models.Entity;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class UpdateUserSubscriptionController : Controller
    {
        private readonly IUpdateUserSubscriptionPlan _updateUserSubscriptionPlan;
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdateUserSubscriptionController(IUpdateUserSubscriptionPlan updateUserSubscriptionPlan, UserManager<ApplicationUser> userManager)
        {
            _updateUserSubscriptionPlan = updateUserSubscriptionPlan;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task UpgradeUserSubscription()
        {
            var user = await _userManager.GetUserAsync(User);
            await _updateUserSubscriptionPlan.UpgradeSubscriptionPlan(user);
        }

        public async Task DowngradeUserSubscription()
        {
            var user = await _userManager.GetUserAsync(User);
            await _updateUserSubscriptionPlan.DowngradeSubscriptionPlan(user);
        }

    }
}
