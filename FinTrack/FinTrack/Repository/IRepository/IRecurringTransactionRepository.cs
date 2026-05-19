using FinTrack.Models.Entity;

namespace FinTrack.Repository.IRepository
{
    public interface IRecurringTransactionRepository
    {
        public Task<IEnumerable<RecurringTransaction>> GetAllRecurringTransactionsAsync();
        public Task<RecurringTransaction> GetRecurringTransactionByIdAsync(int id);
        public Task AddRecurringTransactionAsync(RecurringTransaction recurringTransaction);
        public void UpdateRecurringTransactionAsync(RecurringTransaction recurringTransaction);
        public void DeleteRecurringTransactionAsync(RecurringTransaction recurringTransaction);
    }
}
