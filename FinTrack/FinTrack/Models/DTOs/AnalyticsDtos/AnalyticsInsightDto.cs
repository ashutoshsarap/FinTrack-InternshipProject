namespace FinTrack.Models.DTOs.AnalyticsDtos
{
    public class AnalyticsInsightDto
    {

        public DateTime DateSpentMostOn { get; set; }
        public decimal AmountSpentOnThatDay { get; set; }
        public string CategoryWithHighestExpense { get; set; }
        public decimal AmountSpentInHighestCategory { get; set; }
        public HighestExpenseInfo? HighestExpenseInfo { get; set; }
    }
}
