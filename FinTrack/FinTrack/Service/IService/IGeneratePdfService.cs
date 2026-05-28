using FinTrack.Models;

namespace FinTrack.Service.IService
{
    public interface IGeneratePdfService
    {
        public byte[] GeneratePdfReportForMonthlyAnalytics(string userId, MonthlyReport monthlyReport);
    }
}
