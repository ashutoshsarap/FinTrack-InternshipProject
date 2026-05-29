using FinTrack.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult RedirectUser()
            {
            if(User.IsInRole(Roles.Admin))
            {
                return RedirectToAction("Index", "Admin");
            }
            else 
            {
                return RedirectToAction("Index", "Dashboard");
            }
            
        }
    }
}
