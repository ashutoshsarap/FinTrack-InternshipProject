using FinTrack.Models.DTOs.RecurringTransactionDtos;
using FinTrack.Models.Entity;
//V2
namespace FinTrack.Repository.IRepository
{
    public interface IRecurringTransactionRepository
    {
        public Task<IEnumerable<RecurringTransaction>> FindAllRecurringTransactionsAsync();
        public Task<RecurringTransaction> FindUserSpecificRecurringTransactionByIdAsync(int id);
        public Task AddRecurringTransactionAsync(RecurringTransaction recurringTransaction);
        public void UpdateRecurringTransaction(RecurringTransaction recurringTransaction);
        public void DeleteRecurringTransaction(RecurringTransaction recurringTransaction);
        //Task<IEnumerable<RecurringTransaction>> FindAllPendingRecurringTransactionsForJobAsync();
        Task<RecurringTransaction> FindRecurringTransactionByIdAsync(int id);
    }
}
