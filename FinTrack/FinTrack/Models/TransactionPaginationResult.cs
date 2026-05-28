using FinTrack.Models.Entity;

namespace FinTrack.Models
{
    public class TransactionPaginationResult
    {
        public IEnumerable<Transaction> AllTransactions { get; set; }
        public int TotalCountOfTransactions { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCountOfTransactions / PageSize);

    }
}
