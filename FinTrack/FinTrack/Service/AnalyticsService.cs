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
            DateTime today = DateTime.Today;
            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);
            DateTime previousMonthStart = currentMonthStart.AddMonths(-1);
            DateTime previousMonthEnd = currentMonthStart.AddDays(-1);
            var daysPassed = today.Day;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            //Calculating data for analytics
            var previousMonthExpense = _unitOfWork.Transaction.FindTotalExpenseByMonth(currentMonthStart,currentMonthEnd);
            var currentMonthExpense = _unitOfWork.Transaction.FindTotalExpenseByMonth(previousMonthStart, previousMonthEnd);

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
            DateTime currentMonthYearInfo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);    

            var categoryBreakdown = await _unitOfWork.Transaction.FindCategoryBreakdown(currentMonthYearInfo);
            return categoryBreakdown;
        }

        public AnalyticsInsightDto GetAnalyticsInsight()
        {
            var insight = _unitOfWork.Transaction.FindAnalyticsInsight();
            return insight;
        }
    }
}
