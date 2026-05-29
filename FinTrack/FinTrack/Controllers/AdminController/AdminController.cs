using FinTrack.Models.AdminModelAndDtos.AdminViewModels;
using FinTrack.Service.AdminServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.AdminController
{
    public class AdminController : Controller
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public IActionResult Index()
        {
            var dashboardData = _adminService.GetAdminDashboardData();

            AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel
            {
                TotalUsers = dashboardData.TotalUsers,
                TotalTransactions = dashboardData.TotalTransactions,
                TotalTransactionsThisMonth = dashboardData.TotalTransactionsThisMOnth
            };

            return View(adminDashboardViewModel);
        }
    }
}
