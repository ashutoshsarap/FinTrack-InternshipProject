using FinTrack.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public IActionResult Index()
        {
            var analyticsData = _analyticsService.GetAnalyticsDataAsync();
            return View(analyticsData);
        }
    }
}
