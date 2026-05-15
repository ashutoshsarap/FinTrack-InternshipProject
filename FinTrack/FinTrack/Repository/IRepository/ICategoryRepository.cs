using FinTrack.Models.Entity;
using System.Linq.Expressions;

namespace FinTrack.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task Update(Category category);
        public Task Delete(Category category);
        public Category FindCategoryByFilter(Expression<Func<Category, bool>> filter);
    }
}
