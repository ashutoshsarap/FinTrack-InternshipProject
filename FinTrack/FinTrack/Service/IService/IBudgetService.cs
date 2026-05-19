using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.Entity;
//V1
namespace FinTrack.Service.IService
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetResponseDto>> GetAllBudgetsByMonthAsync(int month, int year);
        Task CreateBudgetAsync(BudgetCreateDto budget);
        Task UpdateBudget(BudgetUpdateDto budget);
        Task DeleteBudget(int budgetId);
        Task<BudgetResponseDto> GetBudgetByIdAsync(int budgetId);
        Task<List<BudgetAnalyticsDto>> GetBudgetAnalytics();
    }
}
