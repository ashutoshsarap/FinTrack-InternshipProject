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
        private readonly IEmailSender _emailSender;
        public DashboardController(ITransactionService transactionService, IBudgetService budgetService, IEmailSender emailSender)
        {
            _transactionService = transactionService;
            _budgetService = budgetService;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = await _transactionService.GetDashboardData();
            dashboardViewModel.BudgetAnalytics = await _budgetService.GetBudgetAnalytics();

            string userEmail = "ashutoshsarap00@gmail.com";
            string subject = "test";
            string message = "This is a test email from FinTrack.";
            await _emailSender.SendEmailAsync(userEmail, subject, message);
            return View(dashboardViewModel);
        }
    }
}
