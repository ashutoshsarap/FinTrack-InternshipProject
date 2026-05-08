
using FinTrack.Models.Entity;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string userId, string? includeProperties);
        public Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string userId, string? includeProperties);
        public Task Update(Transaction transaction);
        public void Delete(Transaction transaction);
    }
}
