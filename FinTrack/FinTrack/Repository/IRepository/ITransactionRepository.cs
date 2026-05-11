
using FinTrack.Models.DTOs;
using FinTrack.Models.Entity;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string? includeProperties);
        public Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(TransactionFilterDto filter, string? includeProperties);
        public void Update(Transaction transaction);
        public void Delete(Transaction transaction);
    }
}
