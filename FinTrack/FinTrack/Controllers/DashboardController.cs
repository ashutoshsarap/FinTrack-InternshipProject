using FinTrack.Models.ViewModels;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinTrack.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ITransactionService _transactionService;

        public DashboardController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = await _transactionService.GetDashboardData();
            return View(dashboardViewModel);
        }
    }
}
