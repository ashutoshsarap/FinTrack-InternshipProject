using FinTrack.Data;
using FinTrack.Models.DTOs.CategoryDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCategory(string userId,CategoryDto category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(category));
            }
            Category categoryEntity = new Category
            {
                Name = category.Name,
                ApplicationUserId = userId,
                IsSystemDefined = false
            };
            await _unitOfWork.Category.CreateAsync(categoryEntity);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Category.FindAllAsync(null);
            return categories;
        }

        public async Task DeleteCategory(int id)
        {
            
            var category = await _unitOfWork.Category.FindAsync(id, null);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            await _unitOfWork.Category.Delete(category);
            await _unitOfWork.Save();
        }
    }

}
