namespace FinTrack.Models.DTOs.AnalyticsDtos
{
    public class CategoryBreakdownDto
    {
        public string CategoryName { get; set; }
        public decimal TotalAmountSpentCurrentMonth { get; set; }
        public decimal TotalAmountSpentPreviousMonth { get; set; }
        public float PercentageOfTotal { get; set; }

        // This property calculates the percentage change from the previous month to the current month
        public float PercentageChangeFromPreviousMonth { get; set; }
    }
}
