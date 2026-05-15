using FinTrack.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class CsvController : Controller
    {
        private readonly ICsvExportService _csvExportService;    
        private readonly ICsvImportService _csvImportService;
        public CsvController(ICsvExportService csvExportService, ICsvImportService csvImportService)
        {
            _csvExportService = csvExportService;
            _csvImportService = csvImportService;
        }
        
        public async Task<IActionResult> DownloadCsv()
        {
            var csvStream = await _csvExportService.GenerateCsv();

            //Send stream bytes the browser as a downloadable file with the specified content type and filename. The browser will prompt the user to download the file instead of trying to display it.
            return File(csvStream, "text/csv", "transactions.csv");
        }

        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile)
        {
            if(csvFile == null || csvFile.Length==0)
            {
                TempData["Error"] = "No file found, Upload a file";
                return RedirectToAction(nameof(Index), "Transaction");
            }

            try
            {
                await _csvImportService.ImportCsv(csvFile);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error importing CSV: {ex.Message}";
                return RedirectToAction(nameof(Index), "Transaction");
            }

            return RedirectToAction(nameof(Index), "Transaction");
        }
    }
}
