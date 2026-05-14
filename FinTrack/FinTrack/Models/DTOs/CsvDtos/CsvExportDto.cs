using CsvHelper.Configuration.Attributes;
using FinTrack.Models.Enums;

namespace FinTrack.Models.DTOs.CsvDtos
{
    public class CsvExportDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string PaymentMode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
