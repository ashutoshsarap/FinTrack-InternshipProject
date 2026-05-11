using FinTrack.Models.DTOs;
using FinTrack.Models.Entity;

namespace FinTrack.Service.IService
{
    public interface ICategoryService
    {
        //public Task CreateCategoryAsync(Category categoryCreateDto);
        //public Task UpdateCategory(int id, Category categoryUpdateDto);
        //public Task DeleteCategory(int id);
        //public Task<Category> GetCategoryByIdAsync(int id);

        public Task CreateCategory(string userId,CategoryDto category);
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task DeleteCategory(int id);
    }
}
