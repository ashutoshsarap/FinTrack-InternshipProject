using FinTrack.Models.DTOs;
using FinTrack.Models.DTOs.BudgetDtos;

namespace FinTrack.Models.ViewModels
{
    public class DashboardViewModel
    {

        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetBalance { get; set; }
        public List<CategoryExpenseDto> ExpenseCategorySummaries { get; set; }
        //Recent transactions for the dashboard shows last 5 transactions
        public List<TransactionResponseDto> RecentTransactions { get; set; }
        public List<BudgetAnalyticsDto> BudgetAnalytics { get; set; }

    }
}
