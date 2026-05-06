//V1
using FinTrack.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public PaymentMode PaymentMode { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [ValidateNever]
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public SelectList Categories { get; set; }
    }
}
