using FinTrack.Models.DTOs;
using FinTrack.Models.Entity;
using System.Linq.Expressions;
//V1
namespace FinTrack.Service.IService
{
    public interface ITransactionService
    {
        public Task CreateTransactionAsync(TransactionCreateDto transactionCreateDto);
        public Task UpdateTransaction(int id, TransactionUpdateDto transactionUpdateDto);
        public Task DeleteTransaction(int id);
        public Task<TransactionResponseDto> GetTransactionByIdAsync(int id, string includeProperties);
        public Task<List<TransactionResponseDto>> GetAllTransactionsAsync();
        public Task<List<TransactionResponseDto>> GetTransactionsByFilterAsync(Expression<Func<Transaction, bool>> filter);
        public Task<TransactionResponseDto> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter);

    }
}
