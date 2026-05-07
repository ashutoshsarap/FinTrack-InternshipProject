using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.ViewModels;
using FinTrack.Repository;
using FinTrack.Repository.IRepository;
using FinTrack.Service;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

//V1
namespace FinTrack.Controllers
{
    //[Authorize]
    public class TransactionController : Controller
    {

        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        public TransactionController(ApplicationDbContext context, ITransactionService transactionService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetTransactionsByFilterAsync(t => !t.IsDeleted);
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
            var categories = await _categoryService.GetAllCategoriesAsync();

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
                    await _transactionService.CreateTransactionAsync(transactionCreateDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the transaction: {ex.Message}");
                }
            }
            return View(model);

        }

        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _transactionService.DeleteTransaction(id);
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
            var transaction = await _transactionService.GetTransactionByIdAsync(id, includeProperties : "Category");
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
            var categories = await _categoryService.GetAllCategoriesAsync();

            if (!ModelState.IsValid)
            {
                //var categories = _context.Categories.ToList();
                model.Categories = new SelectList(categories.ToList(), "Id", "Name");
                return View(model);
            }


            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(model.Id, null);

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
                await _transactionService.UpdateTransaction(model.Id, transactionUpdateDto);
                return RedirectToAction(nameof(Index));

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
