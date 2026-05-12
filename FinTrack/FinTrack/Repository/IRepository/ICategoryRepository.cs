using FinTrack.Models.Entity;

namespace FinTrack.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task Update(Category category);
        public Task Delete(Category category);
    }
}
