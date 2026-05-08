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

        public async Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string userId, string? includeProperties)
        {
            return await _dbContext.Transactions.Include(includeProperties)
                                                .Where(t => t.ApplicationUserId == userId)
                                                .Where(filter)
                                                .Where(t=>!t.IsDeleted)
                                                .ToListAsync();
        }

        public async Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string userId, string? includeProperties)
        {
            return await _dbContext.Transactions.Include(includeProperties)
                                                .Where(t => t.ApplicationUserId == userId)
                                                .Where(filter)
                                                .Where(t => !t.IsDeleted)
                                                .FirstOrDefaultAsync();
        }

        public async Task Update(Transaction transaction)
        {
            var existingTransaction = await _dbContext.Transactions.FindAsync(transaction.Id);

            if (existingTransaction != null)
            {
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Date = transaction.Date;
                existingTransaction.Type = transaction.Type;
                existingTransaction.PaymentMode = transaction.PaymentMode;
                existingTransaction.Description = transaction.Description;
                existingTransaction.UpdatedAt = DateTime.Now;
                existingTransaction.CategoryId = transaction.CategoryId;
            }

        }
    }
}
