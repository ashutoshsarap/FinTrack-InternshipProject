using FinTrack.Models.DTOs.RecurringTransactionDtos;
using FinTrack.Models.Entity;

namespace FinTrack.Repository.IRepository
{
    public interface IRecurringTransactionRepository
    {
        public Task<IEnumerable<RecurringTransaction>> FindAllRecurringTransactionsAsync();
        public Task<RecurringTransaction> FindRecurringTransactionByIdAsync(int id);
        public Task AddRecurringTransactionAsync(RecurringTransaction recurringTransaction);
        public void UpdateRecurringTransaction(RecurringTransaction recurringTransaction);
        public void DeleteRecurringTransaction(RecurringTransaction recurringTransaction);
    }
}
