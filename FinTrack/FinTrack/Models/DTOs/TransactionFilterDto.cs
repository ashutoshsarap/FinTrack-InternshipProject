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

        //Pagination properties
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } =10; // Default page size
    }
}
