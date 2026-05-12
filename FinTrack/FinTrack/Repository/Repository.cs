using FinTrack.Data;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Microsoft.EntityFrameworkCore;
//V2
namespace FinTrack.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        private readonly string _currentUserId;
        public Repository(ApplicationDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            dbSet = _db.Set<T>();
            _currentUserId = currentUserService.UserId;
        }

        //Add transaction to the database
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        //Fetch all transactions from the database
        public async Task<IEnumerable<T>> FindAllAsync(string? includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (includeProperties != null)
            {
                query= query.Include(includeProperties);
            }
            if (typeof(T).Equals(typeof(FinTrack.Models.Entity.Category)))
            {
                return query.Where(e => EF.Property<string>(e, "ApplicationUserId") == _currentUserId || EF.Property<bool>(e, "IsSystemDefined") == true ).ToList();
            }
            return query.Where(e => EF.Property<string>(e,"ApplicationUserId")==_currentUserId).ToList();
        }

        //Fetch a transaction by id from the database
        public async Task<T> FindAsync(int id, string? includeProperties)
        {
            IQueryable<T> query = dbSet;


            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }


            // e => EF.Property<int>(e, "Id") == id ->  Dynamically access the "Id" property of the entity using EF.Property
            //Equivalent to e => e.Id == id, but allows for more flexibility when the property name is not known at compile time.
            //EF -> Static class that provides methods for working with Entity Framework Core, including the EF.Property method used here to access properties dynamically.
            //Property<int>(e, "Id") -> Accesses the "Id" property of the entity e and treats it as an integer. This allows the code to work with entities that may not have a strongly-typed Id property or when the property name is determined at runtime.
            //This approach can fail if the entity does not have an "Id" property or if the property is not of type int
            var entity = await query
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id && EF.Property<string>(e, "ApplicationUserId") == _currentUserId);
            return entity;
        }

    }
}
