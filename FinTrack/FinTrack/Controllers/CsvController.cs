using FinTrack.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class CsvController : Controller
    {
        private readonly IGenerateCsvService _csvService;    

        public CsvController(IGenerateCsvService csvService)
        {
            _csvService = csvService;
        }
        

        public async Task<IActionResult> DownloadCsv()
        {
            var csvStream = await _csvService.GenerateCsv();
            return File(csvStream, "text/csv", "transactions.csv");
        }

    }
}
