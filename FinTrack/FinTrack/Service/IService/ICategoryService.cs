using FinTrack.Models.DTOs.CategoryDtos;
using FinTrack.Models.Entity;
using System.Reflection.Metadata;

namespace FinTrack.Service.IService
{
    public interface ICategoryService
    {
        //public Task CreateCategoryAsync(Category categoryCreateDto);
        //public Task UpdateCategory(int id, Category categoryUpdateDto);
        //public Task DeleteCategory(int id);
        //public Task<Category> GetCategoryByIdAsync(int id);

        public Task CreateCategory(string userId, string userName,CategoryDto category);
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task DeleteCategory(string userName, int id);
    }
}
