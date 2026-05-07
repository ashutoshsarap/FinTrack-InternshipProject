
using FinTrack.Models.Entity;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<Transaction> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter);
        public Task<IEnumerable<Transaction>> GetAllTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter);
        public void UpdateTransaction(Transaction transaction);
        public void DeleteTransaction(Transaction transaction);
    }
}
