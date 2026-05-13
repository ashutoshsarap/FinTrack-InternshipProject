using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
//V1
namespace FinTrack.Service
{
    public class AnalyticsService : IAnalyticsService
    {

        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsDto GetAnalyticsData()
        {
            //Calculating date related values
            var today = DateTime.Today;
            var previousMonth = DateTime.Now.AddMonths(-1).Month;
            var currentMonth = today.Month;
            var daysPassed = today.Day;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            //Calculating data for analytics
            var previousMonthExpense = _unitOfWork.Transaction.FindTotalExpenseByMonth(previousMonth);
            var currentMonthExpense = _unitOfWork.Transaction.FindTotalExpenseByMonth(currentMonth);

            var ExpensePercentageChange = previousMonthExpense == 0 ? 0 : ((currentMonthExpense - previousMonthExpense) / previousMonthExpense) * 100;

            var averageDailyExpense = currentMonthExpense / daysInMonth;
            var averageWeeklyExpense = averageDailyExpense * 7;

            //Predicted monthly expense = S + (D * R)
            //S: Amount Spent so far this month. D: Average spent everyday. R: Remaining days in the month
            var predictedMonthlyExpense = currentMonthExpense + (averageDailyExpense * (daysInMonth - daysPassed));

            var analyticsData = new AnalyticsDto
            {
                CurrentMonthExpense = currentMonthExpense,
                PreviousMonthExpense = previousMonthExpense,
                ExpensePercentageChange = (float)Math.Round(ExpensePercentageChange, 2),
                AverageDailyExpense = averageDailyExpense,
                AverageWeeklyExpense = averageWeeklyExpense,
                PredictedMonthlyExpense = predictedMonthlyExpense
            };

            return analyticsData;
        }

        public async Task<List<CategoryBreakdownDto>> GetCategoryBreakdown()
        {

            //Calculating date related values
            var today = DateTime.Today;
            var previousMonth = DateTime.Now.AddMonths(-1).Month;
            var currentMonth = today.Month;
            var daysPassed = today.Day;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            var categoryBreakdown = await _unitOfWork.Transaction.FindCategoryBreakdown(previousMonth, currentMonth);
            return categoryBreakdown;
        }

        public AnalyticsInsightDto GetAnalyticsInsight()
        {
            var insight = _unitOfWork.Transaction.FindAnalyticsInsight();
            return insight;
        }
    }
}
