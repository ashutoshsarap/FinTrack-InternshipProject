using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public void DeleteTransaction(Transaction transaction)
        {
            var existingTransaction = _dbContext.Transactions.FirstOrDefault(t => t.Id == transaction.Id);
            if (existingTransaction != null)
            {
                existingTransaction.IsDeleted = true;
                existingTransaction.DeletedAt = DateTime.Now;
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            return await _dbContext.Transactions.Where(filter)
                                                .Where(t=>!t.IsDeleted)
                                                .ToListAsync();
        }

        public async Task<Transaction> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            return await _dbContext.Transactions.Where(filter)
                                                .Where(t => !t.IsDeleted)
                                                .FirstOrDefaultAsync();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var existingTransaction = _dbContext.Transactions.FirstOrDefault(t => t.Id == transaction.Id);

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
