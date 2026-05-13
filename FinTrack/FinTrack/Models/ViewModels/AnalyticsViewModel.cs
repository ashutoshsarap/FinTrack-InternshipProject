using FinTrack.Models.DTOs.AnalyticsDtos;

namespace FinTrack.Models.ViewModels
{
    public class AnalyticsViewModel
    {
        public AnalyticsDto Analytics { get; set; }
        public List<CategoryBreakdownDto> CategoryBreakdown { get; set; }
        public AnalyticsInsightDto AnalyticsInsight { get; set; }
    }
}
