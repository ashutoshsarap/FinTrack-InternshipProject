using FinTrack.Models.DTOs;
using FinTrack.Models.Entity;
using System.Linq.Expressions;
//V1
namespace FinTrack.Service.IService
{
    public interface ITransactionService
    {
        public Task CreateTransactionAsync(TransactionCreateDto transactionCreateDto);
        public Task UpdateTransactionAsync(TransactionUpdateDto transactionUpdateDto);
        public Task DeleteTransactionAsync(int id);

        public Task<TransactionResponseDto> GetTransactionByIdAsync(int id);
        public Task<List<TransactionResponseDto>> GetAllTransactionsAsync();
        public Task<List<TransactionResponseDto>> GetTransactionsByFilterAsync(Expression<Func<Transaction, bool>> filter);
        public Task<TransactionResponseDto> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter);

    }
}
