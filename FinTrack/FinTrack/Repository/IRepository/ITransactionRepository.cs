
using FinTrack.Models.DTOs;
using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using System.Linq.Expressions;
//V1
namespace FinTrack.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string? includeProperties);
        public Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(TransactionFilterDto filter, string? includeProperties);
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
    }
}
