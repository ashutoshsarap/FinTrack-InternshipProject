using FinTrack.Models.Entity;

namespace FinTrack.Models.DTOs.BudgetDtos
{
    public class BudgetResponseDto
    {
        public int Id { get; set; }
        public decimal MonthlyLimitAmount { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
