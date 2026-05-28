using FinTrack.Models;
using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.DTOs.BudgetDtos;

namespace FinTrack.Service.IService
{
    public interface IAnalyticsService
    {

        public AnalyticsDto GetAnalyticsData();
        public Task<List<CategoryBreakdownDto>> GetCategoryBreakdown();
        public AnalyticsInsightDto GetAnalyticsInsight();
        Task<List<MonthlyExpenseTrendAnalyticsDto>> GetMonthlyExpenseTrends();
        Task<MonthlyReport> GetMonthlyReport(string userId);

    }
}
