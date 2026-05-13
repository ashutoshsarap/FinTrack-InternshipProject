using FinTrack.Models.DTOs.AnalyticsDtos;

namespace FinTrack.Service.IService
{
    public interface IAnalyticsService
    {

        public AnalyticsDto GetAnalyticsData();
        public Task<List<CategoryBreakdownDto>> GetCategoryBreakdown();
        public AnalyticsInsightDto GetAnalyticsInsight();

    }
}
