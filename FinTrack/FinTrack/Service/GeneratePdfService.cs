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
                    //Configure page settings

                    //Configures the page size, margins, background color, and default text style
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.Header()
                        .Text($"Monthly Financial Report - {monthName} {year}")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                    //Represents the main content of the page, which is organized in a column layout, located below the header and above the footer. It contains the financial data for the month, such as total income, total expenses, and net savings.
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

            //Generates the PDF document and returns it as a byte array, which can be used for downloading or further processing.
            return document.GeneratePdf();
        }
    }
}
