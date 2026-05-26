using FinTrack.Models.ViewModels;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinTrack.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IBudgetService _budgetService;
        public DashboardController(ITransactionService transactionService, IBudgetService budgetService, IEmailSender emailSender)
        {
            _transactionService = transactionService;
            _budgetService = budgetService;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = await _transactionService.GetDashboardData();
            dashboardViewModel.BudgetAnalytics = await _budgetService.GetBudgetAnalytics();

            return View(dashboardViewModel);
        }
    }
}
