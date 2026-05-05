//V1
namespace FinTrack.Models.DTOs
{
    public class TransactionUpdateDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string PaymentMode { get; set; } 
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
