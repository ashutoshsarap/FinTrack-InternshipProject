using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.ViewModels;
using FinTrack.Service.IService;
using FinTrack.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinTrack.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class BudgetController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly ICategoryService _categoryService;
        public BudgetController(IBudgetService budgetService, ICategoryService categoryService)
        {
            _budgetService = budgetService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var budgetInsights = await _budgetService.GetBudgetAnalytics();

            List<BudgetCardViewModel> budgetCards = budgetInsights.Select(b => new BudgetCardViewModel
            {
                Id = b.BudgetId,
                CategoryName = b.CategoryName,
                MonthlyLimitBudgetAmount = b.MonthlyLimitAmount,
                TotalAmountSpent = b.TotalAmountSpent,
                RemainingAmount = b.RemainingAmount,
                PercentageUsed = b.PercentageUsed,
                IsOverBudget = b.IsOverBudget
            }).ToList();

            return View(budgetCards);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BudgetViewModel model)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (ModelState.IsValid)
            {
                try
                {

                    BudgetCreateDto budgetCreateDto = new BudgetCreateDto
                    {
                        MonthlyLimitAmount = model.MonthlyLimitAmount,
                        CategoryId = model.CategoryId
                    };

                    await _budgetService.CreateBudgetAsync(budgetCreateDto);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while creating the budget. Please try again.";
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the budget: {ex.Message}");
                    ViewBag.Categories = categories;
                    return View(model);
                }
            }
            ViewBag.Categories = categories;
            return View(model);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _budgetService.DeleteBudget(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the transaction: {ex.Message}");
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            BudgetResponseDto budgetResponseDto;
            try
            {
                 budgetResponseDto = await _budgetService.GetBudgetByIdAsync(id);
            }
            catch(Exception ex)
            {
                return NotFound();
            }


            BudgetViewModel budgetViewModel = new BudgetViewModel
            {
                Id=budgetResponseDto.Id,
                MonthlyLimitAmount = budgetResponseDto.MonthlyLimitAmount,
                CategoryName = budgetResponseDto.CategoryName,
                CategoryId = budgetResponseDto.CategoryId
            };

            return View(budgetViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BudgetViewModel budgetViewModel)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            try
            {
                BudgetUpdateDto budgetUpdateDto = new BudgetUpdateDto
                {
                    Id = budgetViewModel.Id,
                    MonthlyLimitAmount = budgetViewModel.MonthlyLimitAmount,
                    CategoryId = budgetViewModel.CategoryId
                };

                await _budgetService.UpdateBudget(budgetUpdateDto);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the budget: {ex.Message}");
            }

            return View(budgetViewModel);
        }

        
    }
}
