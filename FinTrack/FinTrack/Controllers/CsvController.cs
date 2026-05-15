using FinTrack.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FinTrack.Controllers
{
    [Authorize]
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
            var importResult = await _csvImportService.ImportCsv(csvFile);

            TempData["ImportResult"] = $@"
                                        <ul>
                                            <li><strong>Records Imported:</strong> {importResult.RecordsImported}</li>
                                            <li><strong>Total Added:</strong> {importResult.TotalRecordsAdded}</li>
                                            <li><strong>Duplicates Found:</strong> {importResult.DuplicateRecordsFound}</li>
                                            <li><strong>Invalid Records:</strong> {importResult.InvalidRecordsFound}</li>
                                        </ul>

                                        <strong>Errors:</strong>
                                        <ul>
                                            {string.Join("", importResult.Errors.Select(e => $"<li>{e}</li>"))}
                                        </ul>";

            return RedirectToAction(nameof(Index), "Transaction");
        }

        [HttpGet]
        public IActionResult DownloadCsvTemplate()
        {
            var csvBuilder = new StringBuilder();

            csvBuilder.AppendLine("Amount,Date,Type,PaymentMode,Description,CategoryName");

            csvBuilder.AppendLine(
                "500,2026-01-15,Expense,Cash,Lunch,Food");

            csvBuilder.AppendLine(
                "2000,2026-01-16,Income,UPI,Freelance,Salary");

            var bytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(
                bytes,
                "text/csv",
                "Import_Template.csv"
            );
        }
    }
}
