using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
//V1
namespace FinTrack.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(Transaction transaction)
        {
            transaction.IsDeleted = true;
            transaction.DeletedAt = DateTime.Now;
        }

        public async Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string? includeProperties)
        {
            IQueryable<Transaction> query = _dbContext.Transactions;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = query.Include(includeProperties);
            }
            return await query.Where(filter).ToListAsync();
        }

        public async Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string? includeProperties)
        {
            IQueryable<Transaction> query = _dbContext.Transactions;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = query.Include(includeProperties);
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public void Update(Transaction transaction)
        {
            _dbContext.Update(transaction);

        }
    }
}
