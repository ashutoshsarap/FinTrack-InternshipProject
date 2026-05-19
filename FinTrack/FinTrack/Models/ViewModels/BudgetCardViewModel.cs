namespace FinTrack.Models.ViewModels
{
    public class BudgetCardViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public decimal MonthlyLimitBudgetAmount { get; set; }
        public decimal TotalAmountSpent { get; set; }
        public decimal RemainingAmount { get; set; }
        public float PercentageUsed { get; set; }
        public bool IsOverBudget { get; set; }

    }
}
