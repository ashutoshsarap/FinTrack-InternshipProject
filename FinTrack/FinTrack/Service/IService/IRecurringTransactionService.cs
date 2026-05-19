using FinTrack.Models.DTOs.RecurringTransactionDtos;

namespace FinTrack.Service.IService
{
    public interface IRecurringTransactionService
    {
        public Task<IEnumerable<RecurringTransactionResponseDto>> GetAllRecurringTransactionsAsync(string userId);
        public Task<RecurringTransactionResponseDto> GetRecurringTransactionByIdAsync(int id, string userId);
        public Task CreateRecurringTransactionAsync(RecurringTransactionCreateDto recurringTransactionCreateDto);
        public void UpdateRecurringTransactionAsync();
        public void DeleteRecurringTransactionAsync(int id);
    }
}
