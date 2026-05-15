using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FinTrack.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _currentUserId;
        public CategoryRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService) : base(dbContext, currentUserService)
        {
            _dbContext = dbContext;
            _currentUserId = currentUserService.UserId;
        }

        public async Task Update(Category category)
        {
            var existingCategory = await  _dbContext.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
            }
        }

        public Task Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            return Task.CompletedTask;
        }

        public Category FindCategoryByFilter(Expression<Func<Category, bool>> filter)
        {
            var category = _dbContext.Categories
                                     .Where(c => c.ApplicationUserId == _currentUserId || c.IsSystemDefined)
                                     .Where(filter);
            return category.FirstOrDefault();
        }
    }
}
