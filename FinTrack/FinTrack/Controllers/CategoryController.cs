using FinTrack.Models.DTOs.CategoryDtos;
using FinTrack.Service.IService;
using FinTrack.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly ICurrentUserService _currentUserService;
        public CategoryController(ICategoryService categoryService, ICurrentUserService currentUserService)
        {
            _categoryService = categoryService;
            _currentUserService = currentUserService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoriesList = categories.Where(c=> c.IsSystemDefined==false).Select(c => new CategoryDto()
            {
                Id = c.Id,
                Name = c.Name
            });
            return View(categoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            string userId = _currentUserService.UserId;
            string userName = _currentUserService.UserName; 

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.CreateCategory(userId, userName, category);
                    HttpContext.Items[AuditMessages.AuditMessage] = AuditMessages.CreatedCategory;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the category: {ex.Message}");
                }
            }
            return View(category);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            string userName = _currentUserService.UserName;
            try
            {
                await _categoryService.DeleteCategory(userName,id);
                HttpContext.Items[AuditMessages.AuditMessage] = AuditMessages.DelteCategory;
                TempData["Success"] = "Category deleted successfully";
                return Json(new { success = true, message = "Category deleted successfuly" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Cannot delete category since there is/are transactions with this category";
                return Json(new { success = false, message = "Category not deleted, something went wrong"});

            }
        }
    }
}
