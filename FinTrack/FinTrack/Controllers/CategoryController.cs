using FinTrack.Models.DTOs;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinTrack.Controllers
{
    [Authorize]
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
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.CreateCategory(_currentUserService.UserId, category);
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
            try
            {
                await _categoryService.DeleteCategory(id);
                TempData["Success"] = "Category deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Cannot delete category since there is/are transactions with this category";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
