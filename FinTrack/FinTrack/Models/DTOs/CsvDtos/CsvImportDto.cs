namespace FinTrack.Models.DTOs.CsvDtos
{
    public class CsvImportDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
