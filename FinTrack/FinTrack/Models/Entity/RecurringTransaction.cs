using FinTrack.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinTrack.Models.Entity
{
    public class RecurringTransaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public DateTime StartDate { get; set; }
        public TransactionFrequency TransactionFrequency { get; set; }
        [ForeignKey("CategoryId")]       
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
