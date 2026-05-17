using FinTrack.Models.Enums;

namespace FinTrack.Models.DTOs
{
    public class TransactionFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TransactionType? TransactionType { get; set; }
        public PaymentMode? PaymentMode { get; set; }
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; }
    }
}
