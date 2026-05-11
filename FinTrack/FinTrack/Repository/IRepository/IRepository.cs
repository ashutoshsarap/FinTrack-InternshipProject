//V1
namespace FinTrack.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> FindAsync(int id, string? includeProperties);
        public Task<IEnumerable<T>> FindAllAsync(string? includeProperties);
        public Task CreateAsync(T entity);

    }
}
