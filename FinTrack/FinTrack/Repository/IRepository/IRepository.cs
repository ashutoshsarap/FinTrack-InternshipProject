//V1
namespace FinTrack.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> FindAsync(int id, string userId, string? includeProperties);
        public Task<IEnumerable<T>> FindAllAsync(string userId, string includeProperties);
        public Task CreateAsync(T entity);

    }
}
