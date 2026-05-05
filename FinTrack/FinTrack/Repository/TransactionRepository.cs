using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
        }

        public async void DeleteTransactionAsync(Transaction transaction)
        {
            _dbContext.Transactions.Remove(transaction);

        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _dbContext.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(filter);
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _dbContext.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            return await _dbContext.Transactions.Where(filter).ToListAsync();
        }

        public void UpdateTransactionAsync(Transaction transaction)
        {
            _dbContext.Transactions.Update(transaction);
        }
    }
}

//AddASync, Update and Remove method dont actually interact with the database, they just modify the state of the entity in the DbContext, and the actual database update happens when SaveChangesAsync is called.

//ToListAsync, FirstOrDefaultAsync, FindAsync actually interact with the database to fetch the data based on the query.

//FindAsync is used to find an entity by its primary key, and it can take advantage of the DbContext's cache, so if the entity is already tracked by the context, it will return it without making a database call. If the entity is not found in the cache, it will query the database.