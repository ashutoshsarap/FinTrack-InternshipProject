using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.ViewModels;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinTrack.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public async Task<IActionResult> Index()
        {
            AnalyticsDto analyticsDto = _analyticsService.GetAnalyticsData();
            List<CategoryBreakdownDto> categoryBreakdown = await _analyticsService.GetCategoryBreakdown();
            AnalyticsInsightDto analyticsInsight = _analyticsService.GetAnalyticsInsight();
            List<MonthlyExpenseTrendAnalyticsDto> monthlyExpenseTrends = await _analyticsService.GetMonthlyExpenseTrends();
            AnalyticsViewModel analyticsViewModel = new AnalyticsViewModel
            {
                Analytics = analyticsDto,
                CategoryBreakdown = categoryBreakdown,
                AnalyticsInsight = analyticsInsight,
                MonthlyExpenseTrends = monthlyExpenseTrends
            };

            return View(analyticsViewModel);
        }
    }
}
