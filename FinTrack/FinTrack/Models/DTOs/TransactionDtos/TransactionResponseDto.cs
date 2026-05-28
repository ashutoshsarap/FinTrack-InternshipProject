using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
//V1
namespace FinTrack.Models.DTOs.TransactionDto
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public PaymentMode PaymentMode { get; set; } 
        public TransactionFrequency TransactionFrequency { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Category Category { get; internal set; }
        
    }
}
