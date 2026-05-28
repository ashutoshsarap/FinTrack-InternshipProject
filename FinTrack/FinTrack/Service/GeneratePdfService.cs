using FinTrack.Models;
using FinTrack.Service.IService;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FinTrack.Service
{
    public class GeneratePdfService : IGeneratePdfService
    {
        
        public byte[] GeneratePdfReportForMonthlyAnalytics(string userId, MonthlyReport monthlyReport)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            string monthName = DateTime.Now.ToString("MMMM");
            int year = DateTime.Today.Year;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.Header()
                        .Text($"Monthly Financial Report - {monthName} {year}")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);
                            column.Item().Text($"Total Income: {monthlyReport.TotalIncome:C}");
                            column.Item().Text($"Total Expense: {monthlyReport.TotalExpense:C}");
                            column.Item().Text($"Net Savings: {monthlyReport.NetSavings:C}");
                        });
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated on ");
                            x.Span(DateTime.Now.ToString("MMMM dd, yyyy")).SemiBold();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
