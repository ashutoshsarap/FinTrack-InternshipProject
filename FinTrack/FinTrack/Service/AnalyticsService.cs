using FinTrack.Models.DTOs;
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
        public AnalyticsDto GetAnalyticsDataAsync()
        {
            var today = DateTime.Today;
            var previousMonth = DateTime.Now.AddMonths(-1).Month;
            var currentMonth = today.Month;
            var daysPassed = today.Day;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            var previousMonthExpense = _unitOfWork.Transaction.GetTotalExpenseByMonth(previousMonth);
            var currentMonthExpense = _unitOfWork.Transaction.GetTotalExpenseByMonth(currentMonth);

            var ExpensePercentageChange = previousMonthExpense == 0 ? 0 : ((currentMonthExpense - previousMonthExpense) / previousMonthExpense) * 100;

            var averageDailyExpense = currentMonthExpense / daysInMonth;
            var averageWeeklyExpense = averageDailyExpense * 7;
            //var predictedMonthlyExpense = averageDailyExpense * DateTime.DaysInMonth(DateTime.Now.Year, currentMonth);

            //Predicted monthly expense = S + (D * R)
            //S: Amount Spent so far this month. D: Average spent everyday. R: Remaining days in the month
            var predictedMonthlyExpense = currentMonthExpense + (averageDailyExpense * (daysInMonth - daysPassed));

            var analyticsData = new AnalyticsDto
            {
                CurrentMonthExpense = currentMonthExpense,
                PreviousMonthExpense = previousMonthExpense,
                ExpensePercentageChange = (float)Math.Round(ExpensePercentageChange,2),
                AverageDailyExpense = averageDailyExpense,
                AverageWeeklyExpense = averageWeeklyExpense,
                PredictedMonthlyExpense = predictedMonthlyExpense
            };

            return analyticsData;
        }
    }
}
