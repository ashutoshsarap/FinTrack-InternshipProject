using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.DTOs.RecurringTransactionDtos;
using FinTrack.Models.ViewModels;
using FinTrack.Service;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinTrack.Controllers
{
    public class RecurringTransactionController : Controller
    {
        private readonly IRecurringTransactionService _recurringTransactionService;
        private readonly ICategoryService _categoryService;
        public RecurringTransactionController(IRecurringTransactionService recurringTransactionService, ICategoryService categoryService)
        {
            _recurringTransactionService = recurringTransactionService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var allRecurringTransactions = await _recurringTransactionService.GetAllRecurringTransactionsAsync();

            var allRecurringTransactionViewModels = allRecurringTransactions.Select(rt => new RecurringTransactionViewModel
            {
                Id = rt.Id,
                Amount = rt.Amount,
                Description = rt.Description,
                PaymentMode = rt.PaymentMode,
                TransactionFrequency = rt.TransactionFrequency,
                TransactionType = rt.TransactionType,
                CategoryName = rt.CategoryName,
                NextDueDate = rt.NextExecutionDate
            });

            return View(allRecurringTransactionViewModels);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecurringTransactionViewModel model)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    RecurringTransactionCreateDto recurringTransactionCreateDto = new RecurringTransactionCreateDto
                    {
                        Amount = model.Amount,
                        Description = model.Description,
                        PaymentMode = model.PaymentMode,
                        TransactionFrequency = model.TransactionFrequency,
                        TransactionType = model.TransactionType,
                        CategoryId = model.CategoryId,
                        StartDate = model.StartDate
                    };
                    await _recurringTransactionService.CreateRecurringTransactionAsync(recurringTransactionCreateDto);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while creating the Recurring transaction. Please try again.";
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the recurring transaction: {ex.Message}");
                    ViewBag.Categories = categories;
                    return View(model);
                }
            }
            ViewBag.Categories = categories;
            return View(model);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _recurringTransactionService.DeleteRecurringTransaction(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the Recurring transaction. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var recurringTransaction = await _recurringTransactionService.GetRecurringTransactionByIdAsync(id);
            if (recurringTransaction == null)
            {
                return NotFound();
            }
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            RecurringTransactionViewModel model = new RecurringTransactionViewModel
            {
                Id = recurringTransaction.Id,
                Amount = recurringTransaction.Amount,
                Description = recurringTransaction.Description,
                PaymentMode = recurringTransaction.PaymentMode,
                TransactionFrequency = recurringTransaction.TransactionFrequency,
                TransactionType = recurringTransaction.TransactionType,
                CategoryId = recurringTransaction.CategoryId,
                StartDate = recurringTransaction.StartDate
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecurringTransactionViewModel model)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    RecurringTransactionUpdateDto recurringTransactionUpdateDto = new RecurringTransactionUpdateDto
                    {
                        Id = model.Id,
                        Amount = model.Amount,
                        Description = model.Description,
                        PaymentMode = model.PaymentMode,
                        TransactionFrequency = model.TransactionFrequency,
                        TransactionType = model.TransactionType,
                        CategoryId = model.CategoryId,
                        StartDate = model.StartDate
                    };
                    await _recurringTransactionService.UpdateRecurringTransaction(recurringTransactionUpdateDto);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while updating the Recurring transaction. Please try again.";
                    ModelState.AddModelError(string.Empty, $"An error occurred while updating the recurring transaction: {ex.Message}");
                    ViewBag.Categories = categories;
                    return View(model);
                }
            }
            ViewBag.Categories = categories;
            return View(model);
        }
    }
}
