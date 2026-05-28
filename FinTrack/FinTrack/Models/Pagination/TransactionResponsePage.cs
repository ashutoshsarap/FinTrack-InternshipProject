using FinTrack.Models.DTOs.TransactionDto;

namespace FinTrack.Models.Pagination
{
    public class TransactionResponsePage
    {
        public List<TransactionResponseDto> Transactions { get; set; }
        public int TotalCountOfTransactions { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCountOfTransactions / PageSize);

    }
}
