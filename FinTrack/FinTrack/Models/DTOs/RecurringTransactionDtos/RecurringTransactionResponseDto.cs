using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
//V1
namespace FinTrack.Models.DTOs.RecurringTransactionDtos
{
    public class RecurringTransactionResponseDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextExecutionDate { get; set; }
        public TransactionFrequency TransactionFrequency { get; set; }
        public TransactionType TransactionType { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Category Category { get; set; }
    }
}
