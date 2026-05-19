namespace FinTrack.Models.DTOs.BudgetDtos
{
    public class BudgetAnalyticsDto
    {
        public int BudgetId { get; set; }
        public string CategoryName { get; set; }
        public decimal MonthlyLimitAmount { get; set; }
        public decimal TotalAmountSpent { get; set; }
        public decimal RemainingAmount { get; set; }
        public float PercentageUsed { get; set; }
        public bool IsOverBudget { get; set; }
    }
}
