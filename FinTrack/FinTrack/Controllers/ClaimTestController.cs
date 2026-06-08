using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class ClaimTestController : Controller
    {
        [Authorize(Policy = "RequirePremiumSubscription")]
        public IActionResult Index()
        {
            return Json(new { Message = "You have access to this premium content!" });
        }

        [Authorize(Policy = "NonPremiumSubscription")]
        public IActionResult AccessDenied()
        {
            return Json(new { Message = "Access denied. You do not have the required subscription plan." });
        }
    }
}
