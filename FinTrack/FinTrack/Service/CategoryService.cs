using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;

namespace FinTrack.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;   
        public CategoryService(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }


        public void CreateCategory(string userId,CategoryDto category)
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
                ApplicationUserId = userId
            };
            _dbContext.Add(categoryEntity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories(string userId)
        {
            var categories = _dbContext.Categories.Where(c => c.ApplicationUserId == userId).ToList();
            return categories;
        }

        //public async Task CreateCategoryAsync(Category category)
        //{
        //    if (category == null)
        //    {
        //        throw new ArgumentNullException(nameof(category));
        //    }

        //    if (string.IsNullOrWhiteSpace(category.Name))
        //    {
        //        throw new ArgumentException("Category name cannot be empty.", nameof(category));
        //    }

        //    await _unitOfWork.Category.CreateAsync(category);
        //}

        //public async Task DeleteCategory(int id)
        //{
        //    var category = await _unitOfWork.Category.FindAsync(id, null);
        //    if (category == null)
        //    {
        //        throw new KeyNotFoundException($"Category with id {id} not found.");
        //    }
        //    _unitOfWork.Category.Delete(category);
        //}

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId)
        {
            var categories = await _unitOfWork.Category.FindAllAsync(userId, null);
            return categories;
        }

        //public Task<Category> GetCategoryByIdAsync(int id)
        //{
        //    var category = _unitOfWork.Category.FindAsync(id, null);
        //    if (category == null)
        //    {
        //        throw new KeyNotFoundException($"Category with id {id} not found.");
        //    }
        //    return category;
        //}

        //public async Task UpdateCategory(int id, Category category)
        //{
        //    if (category == null)
        //    {
        //        throw new ArgumentNullException(nameof(category));
        //    }
        //    if (string.IsNullOrWhiteSpace(category.Name))
        //    {
        //        throw new ArgumentException("Category name cannot be empty.", nameof(category));
        //    }

        //    var existingCategory = await _unitOfWork.Category.FindAsync(id, null);
        //    if (existingCategory == null)
        //    {
        //        existingCategory.Name=category.Name;
        //    }

        //    _unitOfWork.Category.Update(existingCategory);

        //}
    }

}
