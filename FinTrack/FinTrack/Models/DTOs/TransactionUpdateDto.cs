//V1
using FinTrack.Models.Enums;

namespace FinTrack.Models.DTOs
{
    public class TransactionUpdateDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public PaymentMode PaymentMode { get; set; } 
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
