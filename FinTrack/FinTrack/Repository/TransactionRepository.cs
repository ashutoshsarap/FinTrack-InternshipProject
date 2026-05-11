using FinTrack.Data;
using FinTrack.Models.DTOs;
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

        public async Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(TransactionFilterDto filter, string? includeProperties)
        {
            IQueryable<Transaction> query = _dbContext.Transactions;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = query.Include(includeProperties);
            }
            
            if (filter.StartDate.HasValue)
            {
                query = query.Where(t => t.Date >= filter.StartDate.Value);
            }
            if (filter.EndDate.HasValue)
            {
                query = query.Where(t => t.Date <= filter.EndDate.Value);
            }
            if (filter.TransactionType.HasValue)
            {
                query = query.Where(t => t.Type == filter.TransactionType.Value);
            }
            if (filter.PaymentMode.HasValue)
            {
                query = query.Where(t => t.PaymentMode == filter.PaymentMode.Value);
            }
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            }
            return await query.ToListAsync();
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
