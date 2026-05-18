using FinTrack.CustomExceptions;
using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.Entity;
using FinTrack.Models.ViewModels;
using FinTrack.Repository;
using FinTrack.Repository.IRepository;
using FinTrack.Service;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Threading.Tasks;

//V3
namespace FinTrack.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {

        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrentUserService _currentUser;
        public TransactionController(ApplicationDbContext context, ITransactionService transactionService, ICategoryService categoryService, ICurrentUserService curentUser)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _currentUser = curentUser;
        }
        public async Task<IActionResult> Index(TransactionFilterDto filter)
         {
            // If no date range is provided, default to the current month
            if (!filter.StartDate.HasValue && !filter.EndDate.HasValue)
            {
                var today = DateTime.Today;

                filter.StartDate = new DateTime(today.Year, today.Month, 1);
                filter.EndDate = filter.StartDate.Value.AddMonths(1).AddDays(-1);
            }

            var transactions = await _transactionService.GetAllTransactionsByFilterAsync(filter);
            ViewBag.Filter = filter;
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            var transactionViewModels = transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                PaymentMode = t.PaymentMode,
                TransactionType = t.Type,
                CategoryName = t.CategoryName
            }).ToList();

            return View(transactionViewModels);
        }

        public async Task<IActionResult> Create()
        {
            
            var categories =await _categoryService.GetAllCategoriesAsync();

            var transactionCreateViewModel = new TransactionViewModel
            {
                Date = DateTime.Now,
                Categories = new SelectList(categories, "Id", "Name")
            };

            return View(transactionCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionViewModel model)
        {
            
            var userId = _currentUser.UserId;
            var categories = await _categoryService.GetAllCategoriesAsync();


            if (ModelState.IsValid)
            {
                try
                {
                    var transactionCreateDto = new TransactionCreateDto
                    {
                        Amount = model.Amount,
                        Date = model.Date,
                        Type = model.TransactionType,
                        PaymentMode = model.PaymentMode,
                        Description = model.Description,
                        CategoryId = model.CategoryId
                    };
                    await _transactionService.CreateTransactionAsync(userId,transactionCreateDto);
                    TempData["Success"] = $"Transaction added successfully";
                    return RedirectToAction(nameof(Index), "Dashboard");
                }
                catch(InvalidAmountException ex)
                {
                    TempData["Error"] = $"Invalid amount: {ex.Message}";
                    ModelState.AddModelError(string.Empty, $"Invalid amount: {ex.Message}");
                }
                catch(InvalidDateException ex)
                {
                    TempData["Error"] = $"Invalid date: {ex.Message}";
                    ModelState.AddModelError(string.Empty, $"Invalid date: {ex.Message}");
                }
                catch (DuplicateRecordException ex)
                {
                    TempData["Error"] = $"Transaction already exists";
                    ModelState.AddModelError(string.Empty, $"Transaction already exists");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"An error occurred while creating the transaction: {ex.Message}";
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the transaction: {ex.Message}");
                }
            }
            model.Categories = new SelectList(categories, "Id", "Name");
            return View(model);

        }

        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {

            var userId = _currentUser.UserId;
            try
            {
                await _transactionService.DeleteTransaction(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (RecordNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, $"Transaction not found: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle the error, e.g., log it and show an error message
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the transaction: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _currentUser.UserId;
            var transaction = await _transactionService.GetTransactionByIdAsync(id, userId, includeProperties : "Category");
            if (transaction == null)
            {
                return NotFound();
            }
            var categories = await _categoryService.GetAllCategoriesAsync();
            var transactionEditViewModel = new TransactionViewModel
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                Date = transaction.Date,
                PaymentMode = transaction.PaymentMode,
                TransactionType = transaction.Type,
                CategoryId = transaction.CategoryId,
                Categories = new SelectList(categories.ToList(), "Id", "Name")
            };
            return View(transactionEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransactionViewModel model)
        {
            var userId = _currentUser.UserId;
            var categories = await _categoryService.GetAllCategoriesAsync();
            
            if (!ModelState.IsValid)
            {
                //var categories = _context.Categories.ToList();
                model.Categories = new SelectList(categories.ToList(), "Id", "Name");
                return View(model);
            }


            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(model.Id, userId, null);

                if (transaction == null)
                {
                    return NotFound();
                }

                var transactionUpdateDto = new TransactionUpdateDto
                {
                    Id = model.Id,
                    Amount = model.Amount,
                    Date = model.Date,
                    Type = model.TransactionType,
                    PaymentMode = model.PaymentMode,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    UpdatedAt = DateTime.Now
                };
                //await _transactionService.UpdateTransactionAsync(model.Id, transactionUpdateDto, null);
                await _transactionService.UpdateTransaction(model.Id, userId, transactionUpdateDto);
                return RedirectToAction(nameof(Index));

            }
            catch (RecordNotFoundException ex)
            {
                TempData["Error"] = $"Transaction not found: {ex.Message}";
                ModelState.AddModelError(string.Empty, $"Transaction not found: {ex.Message}");
            }
            catch (InvalidAmountException ex)
            {
                TempData["Error"] = $"Invalid amount: {ex.Message}";
                ModelState.AddModelError(string.Empty, $"Invalid amount: {ex.Message}");
            }
            catch (InvalidDateException ex)
            {
                TempData["Error"] = $"Invalid date: {ex.Message}";
                ModelState.AddModelError(string.Empty, $"Invalid date: {ex.Message}");
            }
            catch (DuplicateRecordException ex)
            {
                TempData["Error"] = $"A transaction with the same details already exists: {ex.Message}";
                ModelState.AddModelError(string.Empty, $"A transaction with the same details already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the transaction: {ex.Message}");
            }

            model.Categories = new SelectList(categories.ToList(), "Id", "Name");
            return View(model);
            
        }


    }
}
