using FinTrack.Service.IService;

namespace FinTrack.Service
{
    public class GenerateReportService : IGenerateReportService
    {
        public string GenerateReport()
        {
            return "Report generated successfully.";
        }
    }
}
