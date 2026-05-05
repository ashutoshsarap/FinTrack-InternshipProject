using FinTrack.Models.Entity;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository.IRepository
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        public Task<Transaction> GetTransactionByIdAsync(int id);
        public Task AddTransactionAsync(Transaction transaction);
        public Task<Transaction> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter);
        public Task<IEnumerable<Transaction>> GetTransactionsByFilterAsync(Expression<Func<Transaction, bool>> filter);

        //Keeping Update and Delete methods as void since the changes are tracked by the DbContext and will be saved when SaveChangesAsync is called 
        //Update and Delete operations dont talk directly with the database, they just modify the state of the entity in the DbContext, and the actual database update happens when SaveChangesAsync is called.
        public void UpdateTransactionAsync(Transaction transaction);
        public void DeleteTransactionAsync(Transaction transaction);
    }
}
