using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;

namespace FinTrack.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(category));
            }

            await _unitOfWork.Category.CreateAsync(category);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _unitOfWork.Category.FindAsync(id, null);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            _unitOfWork.Category.Delete(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Category.FindAllAsync(null);
            return categories;
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = _unitOfWork.Category.FindAsync(id, null);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            return category;
        }

        public async Task UpdateCategory(int id, Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(category));
            }

            var existingCategory = await _unitOfWork.Category.FindAsync(id, null);
            if (existingCategory == null)
            {
                existingCategory.Name=category.Name;
            }

            _unitOfWork.Category.Update(existingCategory);

        }
    }

}
