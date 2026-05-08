//V1
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;

namespace FinTrack.Models.DTOs
{
    public class TransactionUpdateDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public PaymentMode PaymentMode { get; set; } 
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}
