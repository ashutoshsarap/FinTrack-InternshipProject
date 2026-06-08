using FinTrack.Models;
using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.DTOs.CategoryDtos;
using FinTrack.Models.DTOs.TransactionDto;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Models.Pagination;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string? includeProperties);
        public Task<TransactionPaginationResult> FindAllTransactionByFilterAsync(TransactionFilterDto filter, string? includeProperties);
        public Task<IEnumerable<Transaction>> FindAllTransactionForAUser();
        public void Update(Transaction transaction);
        public void Delete(Transaction transaction);
        public Task<List<CategoryExpenseDto>> FindCategoryWiseExpense();
        public Task<List<Transaction>> FindRecentTransactions();
        public decimal FindTotalAmountByTypeAndMonth(TransactionType type, int month, int year);
        public decimal FindTotalExpenseByMonth(DateTime monthStart, DateTime monthEnd);
        public Task<List<CategoryBreakdownDto>> FindCategoryBreakdown(DateTime currentMonthYearInfo);
        public AnalyticsInsightDto FindAnalyticsInsight();
        public Task<bool> IsDuplicateTransaction(Transaction transaction);
        Task<List<MonthlyExpenseTrendAnalyticsDto>> FindlyMonthlyExpenseTrend(int year);
        HighestExpenseInfo FindLargestExpense(DateTime currentMonthStart, DateTime currentMonthEnd);
        Task<MonthlyReport> FindMonthlyReportDataUserSpecific(string userId, int currentMonth, int currentYear);
        Task<List<TransactionResponseDto>> SearchAsync(string userId, string term);
    }
}
