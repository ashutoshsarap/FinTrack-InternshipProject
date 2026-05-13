namespace FinTrack.Models.DTOs.AnalyticsDtos
{
    public class AnalyticsDto
    {
        public decimal CurrentMonthExpense { get; set; }
        public decimal PreviousMonthExpense { get; set; }
        public float ExpensePercentageChange { get; set; }
        public decimal AverageDailyExpense { get; set; }
        public decimal AverageWeeklyExpense { get; set; }
        public decimal PredictedMonthlyExpense { get; set; }
    }
}
