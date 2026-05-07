using FinTrack.Data;
using FinTrack.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
//V1
namespace FinTrack.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await dbSet.ToListAsync();
            return entities;
        }

        public async Task<T> GetAsync(int id, string? includeProperties)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

    }
}
