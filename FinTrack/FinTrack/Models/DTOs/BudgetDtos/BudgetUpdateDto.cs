namespace FinTrack.Models.DTOs.BudgetDtos
{
    public class BudgetUpdateDto
    {
        public int Id { get; set; }
        public decimal MonthlyLimitAmount { get; set; }
        public int CategoryId { get; set; }
    }
}
