using FinTrack.CustomExceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class BrokenTestController : Controller
    {
        public IActionResult Index()
        {
            throw new KeyNotFoundException("An error occurred");
        }
    }
}
