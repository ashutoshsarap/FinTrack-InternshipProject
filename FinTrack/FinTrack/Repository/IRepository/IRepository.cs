//V1
namespace FinTrack.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> GetAsync(int id, string? includeProperties);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task CreateAsync(T entity);



    }
}
