using FinTrack.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Models.DTOs.RecurringTransactionDtos
{
    public class RecurringTransactionCreateDto
    {
        public string? Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public PaymentMode PaymentMode { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public TransactionFrequency TransactionFrequency { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
