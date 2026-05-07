using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using System.Threading.Tasks;

namespace FinTrack.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Update(Category category)
        {
            var existingCategory = await  _dbContext.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
            }
        }

        public void Delete(Category category)
        {
            _dbContext.Remove(category);
        }
    }
}
