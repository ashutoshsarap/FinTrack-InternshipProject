using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.DTOs.BudgetDtos;

namespace FinTrack.Service.IService
{
    public interface IAnalyticsService
    {

        public AnalyticsDto GetAnalyticsData();
        public Task<List<CategoryBreakdownDto>> GetCategoryBreakdown();
        public AnalyticsInsightDto GetAnalyticsInsight();

    }
}
