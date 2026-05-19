using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.Entity;
//V2
namespace FinTrack.Repository.IRepository
{
    public interface IBudgetRepository
    {
            Task<IEnumerable<BudgetResponseDto>> FindAllBudgetsByMonthAsync(int currentMonth, int currentYear);
            Task<Budget> FindBudgetByIdAsync(int id);
            Task CreateBudgetAsync(Budget budget);
            void UpdateBudgetAsync(Budget budget);
            void DeleteBudgetAsync(Budget budget);
            Task<bool> IsDuplicateBudgetAsync(Budget budget);
            Task<List<BudgetAnalyticsDto>> FindBudgetAnalytics();
    }
}
