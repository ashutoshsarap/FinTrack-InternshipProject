using FinTrack.Models.DTOs.RecurringTransactionDtos;
using FinTrack.Models.Entity;

namespace FinTrack.Service.IService
{
    public interface IRecurringTransactionService
    {
        public Task<IEnumerable<RecurringTransactionResponseDto>> GetAllRecurringTransactionsAsync();
        public Task<RecurringTransactionResponseDto> GetRecurringTransactionByIdAsync(int id);
        public Task CreateRecurringTransactionAsync(RecurringTransactionCreateDto recurringTransactionCreateDto);
        public Task UpdateRecurringTransaction(RecurringTransactionUpdateDto recurringTransaction);
        public Task DeleteRecurringTransaction(int id);
    }
}
