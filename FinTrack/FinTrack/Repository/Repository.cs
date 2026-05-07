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
            _db.Transactions.Include(t => t.Category);
        }

        //Add transaction to the database
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        //Fetch all transactions from the database
        public async Task<IEnumerable<T>> FindAllAsync(string includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (includeProperties != null)
            {
                query= query.Include(includeProperties);
            }
            
            return query.ToList();
        }

        //Fetch a transaction by id from the database
        public async Task<T> FindAsync(int id, string? includeProperties)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

    }
}
