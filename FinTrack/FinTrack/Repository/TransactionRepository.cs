using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

//V2
namespace FinTrack.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _currentUserId;
        public TransactionRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService) : base(dbContext, currentUserService)
        {
            _dbContext = dbContext;
            _currentUserId = currentUserService.UserId;
        }

        public void Delete(Transaction transaction)
        {
            transaction.IsDeleted = true;
            transaction.DeletedAt = DateTime.Now;
        }

        //Following method retrieves transactions based on the provided filter criteria. It dynamically builds the query based on which filter properties are set, and includes related entities if specified. Finally, it returns a list of transactions that match the criteria, ordered by date.
        //Used in transaction listing page where user can filter transactions based on date range, type, payment mode and category
        public async Task<IEnumerable<Transaction>> FindAllTransactionByFilterAsync(TransactionFilterDto filter, string? includeProperties)
        {
            IQueryable<Transaction> query = _dbContext.Transactions;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = query.Include(includeProperties);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(t => t.Date >= filter.StartDate.Value);
            }
            if (filter.EndDate.HasValue)
            {
                query = query.Where(t => t.Date <= filter.EndDate.Value);
            }
            if (filter.TransactionType.HasValue)
            {
                query = query.Where(t => t.Type == filter.TransactionType.Value);
            }
            if (filter.PaymentMode.HasValue)
            {
                query = query.Where(t => t.PaymentMode == filter.PaymentMode.Value);
            }
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            }
            return await query.Where(t => t.ApplicationUserId == _currentUserId && t.IsDeleted == false)
                              .OrderBy(t => t.Date)
                              .ToListAsync();
        }

        public async Task<Transaction> FindTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter, string? includeProperties)
        {
            IQueryable<Transaction> query = _dbContext.Transactions;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = query.Include(includeProperties);
            }
            return await query.Where(t => t.ApplicationUserId == _currentUserId).FirstOrDefaultAsync(filter);
        }

        public void Update(Transaction transaction)
        {
            _dbContext.Update(transaction);

        }

        //Following methods are used in dashboard to get total income, total expense, recent transactions and category wise expense for the current month. It uses a helper method GetCurrentMonthDateRange to get the start and end date of the current month and then filters transactions based on that date range and other criteria like transaction type and category.
        public decimal FindTotalAmountByType(TransactionType type)
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var totalAmount = _dbContext.Transactions.Where(t => t.ApplicationUserId == _currentUserId &&
                                                             t.IsDeleted == false &&
                                                             t.Type == type &&
                                                             t.Date >= startDate &&
                                                             t.Date <= endDate)
                                                      .Sum(t => t.Amount);
            return totalAmount;
        }

        public async Task<List<Transaction>> FindRecentTransactions()
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var recentTransactions = await _dbContext.Transactions
                                                     .Include("Category")
                                                     .Where(t => t.ApplicationUserId == _currentUserId &&
                                                            t.IsDeleted == false &&
                                                            t.Date >= startDate &&
                                                            t.Date <= endDate)
                                                     .OrderByDescending(t => t.Date)
                                                     .Take(5)
                                                     .ToListAsync();
            return recentTransactions;
        }

        public async Task<List<CategoryExpenseDto>> FindCategoryWiseExpense()
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var categoryWiseExpense = await _dbContext.Transactions
                                                .Include("Category")
                                                .Where(t => t.ApplicationUserId == _currentUserId &&
                                                            t.IsDeleted == false &&
                                                            t.Type == TransactionType.Expense &&
                                                            t.Date >= startDate &&
                                                            t.Date <= endDate)
                                                .GroupBy(c => c.Category.Name)
                                                .Select(c => new CategoryExpenseDto()
                                                {
                                                    CategoryName = c.Key,
                                                    TotalAmount = c.Sum(t => t.Amount)
                                                })
                                                .ToListAsync();

            return categoryWiseExpense;
        }

        //Returns a tuple containing the start and end date of the current month
        private (DateTime startDate, DateTime endDate) GetCurrentMonthDateRange()
        {
            var today = DateTime.Today;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }

        public decimal FindTotalExpenseByMonth(int month)
        {
            var totalExpenseOfMonth = _dbContext.Transactions.Where(t => t.ApplicationUserId == _currentUserId &&
                                                             t.IsDeleted == false &&
                                                             t.Type == TransactionType.Expense &&
                                                             t.Date.Month == month)
                                                            .Sum(t => t.Amount);
            return totalExpenseOfMonth;
        }


        public async Task<List<CategoryBreakdownDto>> FindCategoryBreakdown(int previousMonth, int currentMonth)
        {

            //Percentage of total = (Amount spent in category / Total expense for the month) * 100

            //Percentage change from previous month = ((Amount spent in category in current month - Amount spent in category in previous month) / Amount spent in category in previous month) * 100

            var totalExpenseCurrentMonth = FindTotalExpenseByMonth(currentMonth);
            var totalExpensePreviousMonth = FindTotalExpenseByMonth(previousMonth);

            var categoryBreakdownList = await _dbContext.Transactions
                .Include("Category")
                .Where(t => t.ApplicationUserId == _currentUserId &&
                            t.IsDeleted == false &&
                            t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryBreakdownDto
                {
                    CategoryName = g.Key,

                    TotalAmountSpentCurrentMonth = g.Where(t => t.Date.Month == currentMonth).Sum(t => t.Amount),

                    TotalAmountSpentPreviousMonth = g.Where(t => t.Date.Month == previousMonth).Sum(t => t.Amount),

                    PercentageOfTotal =
                    totalExpenseCurrentMonth == 0 ? 0 :
                    (float)
                    (g.Where(t => t.Date.Month == currentMonth).Sum(t => t.Amount) / totalExpenseCurrentMonth) * 100,

                    PercentageChangeFromPreviousMonth =
                    g.Where(t => t.Date.Month == previousMonth).Sum(t => t.Amount)==0? 0 :
                    (float)
                    ((g.Where(t => t.Date.Month == currentMonth).Sum(t => t.Amount) - g.Where(t => t.Date.Month == previousMonth).Sum(t => t.Amount)) / g.Where(t => t.Date.Month == previousMonth).Sum(t => t.Amount)) * 100
                })
                .ToListAsync();

            return categoryBreakdownList;
        }

        public AnalyticsInsightDto FindAnalyticsInsight()
        {
            var currentMonth = DateTime.Today.Month;    

            var topCategory = _dbContext.Transactions
                                        .Include("Category")
                                        .Where(t => t.ApplicationUserId == _currentUserId &&
                                                    t.IsDeleted == false &&
                                                    t.Type == TransactionType.Expense &&
                                                    t.Date.Month == currentMonth)
                                        .GroupBy(t => t.Category.Name)
                                        .Select(g => new
                                        {
                                            CategoryName = g.Key,
                                            TotalAmount = g.Sum(t => t.Amount)
                                        })
                                        .OrderByDescending(g => g.TotalAmount)
                                        .FirstOrDefault();

            var highestSpent = topCategory?.CategoryName;
            var amountSpentInHighestCategory = topCategory?.TotalAmount;

            var dateSpentMostOn = _dbContext.Transactions
                                            .Where(t => t.ApplicationUserId == _currentUserId &&
                                                        t.IsDeleted == false &&
                                                        t.Type == TransactionType.Expense &&
                                                        t.Date.Month == currentMonth)
                                            .GroupBy(t => t.Date)
                                            .Select(g => new
                                            {
                                                Date = g.Key,
                                                TotalAmount = g.Sum(t => t.Amount)
                                            })
                                            .OrderByDescending(g => g.TotalAmount)
                                            .FirstOrDefault();

            var dateSpentMost = dateSpentMostOn?.Date;
            var amountSpentOnThatDay = dateSpentMostOn?.TotalAmount;

            AnalyticsInsightDto analyticsInsight = new AnalyticsInsightDto
            {
                CategoryWithHighestExpense = highestSpent?? "Not found",
                AmountSpentInHighestCategory = amountSpentInHighestCategory??0.00m,
                DateSpentMostOn = dateSpentMost ?? DateTime.MinValue,
                AmountSpentOnThatDay = amountSpentOnThatDay ?? 0
            };

            return analyticsInsight;
        }
    }
}
