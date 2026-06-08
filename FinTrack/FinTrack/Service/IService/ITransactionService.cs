using FinTrack.Models.DTOs.TransactionDto;
using FinTrack.Models.Entity;
using FinTrack.Models.Pagination;
using FinTrack.Models.ViewModels;
using System.Linq.Expressions;
//V2
namespace FinTrack.Service.IService
{
    public interface ITransactionService
    {
        public Task CreateTransactionAsync(string userId,string userName,TransactionCreateDto transactionCreateDto);
        public Task UpdateTransaction(int id, string userName, string userId, TransactionUpdateDto transactionUpdateDto);
        public Task DeleteTransaction(int id, string userName, string userId);
        public Task<TransactionResponseDto> GetTransactionByIdAsync(int id, string userId, string includeProperties);
        public Task<List<TransactionResponseDto>> GetAllTransactionsAsync();
        public Task<TransactionResponsePage> GetAllTransactionsByFilterAsync(TransactionFilterDto filterDto);
        public Task<TransactionResponseDto> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter);
        public Task<DashboardViewModel> GetDashboardData();
        Task<List<TransactionResponseDto>> SearchTransactionsAsync(string userId, string term);

    }
}
