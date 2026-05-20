using FinTrack.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Models.ViewModels
{
    public class RecurringTransactionViewModel
    {
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        [Required]
        public TransactionFrequency TransactionFrequency { get; set; }
        [Required]
        public PaymentMode PaymentMode { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ValidateNever]
        public string CategoryName { get; set; }
    }
}
