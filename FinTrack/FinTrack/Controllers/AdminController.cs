using FinTrack.Models.AdminModelAndDtos;
using FinTrack.Models.AdminModelAndDtos.AdminViewModels;
using FinTrack.Models.ViewModels;
using FinTrack.Service.AdminServices.Interfaces;
using FinTrack.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [Authorize(Roles = Roles.Admin)]
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdminViewModel createAdminViewModel)
        {
            if (ModelState.IsValid)
            {
                CreateAdminDto createAdminDto = new CreateAdminDto
                {
                    FullName = createAdminViewModel.FullName,
                    Email = createAdminViewModel.Email,
                    Password = createAdminViewModel.Password
                };

                try
                {
                    await _adminService.CreateAdmin(createAdminDto);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"An error occurred while creating the admin";
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(createAdminViewModel);
                }
                return RedirectToAction("Index");
            }
            return View(createAdminViewModel);
        }
    }
}
