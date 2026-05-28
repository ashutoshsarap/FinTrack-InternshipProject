using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.DTOs.AnalyticsDtos;
using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
            
            query = filter.SortBy switch
            {
                "date_desc" => query.OrderByDescending(t => t.Date),
                "amount_asc" => query.OrderBy(t => t.Amount),
                "amount_desc" => query.OrderByDescending(t => t.Amount),
                _ => query.OrderBy(t => t.Date)
            };
            
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
            
            query = query.Where(t => t.ApplicationUserId == _currentUserId && t.IsDeleted == false);
            
            //Pagination
            int skipPages = (filter.PageNumber - 1) * filter.PageSize;

            query = query.Skip(skipPages).Take(filter.PageSize);

            return await query.ToListAsync();
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
        public decimal FindTotalAmountByTypeAndMonth(TransactionType type, int month, int year)
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var totalAmount = _dbContext.Transactions.Where(t => t.ApplicationUserId == _currentUserId &&
                                                             t.IsDeleted == false &&
                                                             t.Type == type &&
                                                             t.Date.Month == month &&
                                                             t.Date.Year == year
                                                             )
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

        public decimal FindTotalExpenseByMonth(DateTime monthStart, DateTime monthEnd)
        {
            var totalExpenseOfMonth = _dbContext.Transactions.Where(t => t.ApplicationUserId == _currentUserId &&
                                                             t.IsDeleted == false &&
                                                             t.Type == TransactionType.Expense &&
                                                             t.Date >= monthStart &&
                                                             t.Date <= monthEnd)
                                                            .Sum(t => t.Amount);
            return totalExpenseOfMonth;
        }


        public async Task<List<CategoryBreakdownDto>> FindCategoryBreakdown(DateTime currentMonthYearInfo)
        {

            //Percentage of total = (Amount spent in category / Total expense for the month) * 100

            //Percentage change from previous month = ((Amount spent in category in current month - Amount spent in category in previous month) / Amount spent in category in previous month) * 100

            var currentMonthStart = currentMonthYearInfo;
            var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);
            var previousMonthEnd = currentMonthStart.AddDays(-1);

            var baseQuery = _dbContext.Transactions
                                .Include("Category")
                                .Where(t => t.ApplicationUserId == _currentUserId &&
                                            t.IsDeleted == false &&
                                            t.Type == TransactionType.Expense);

            var currentMonthTotalExpense = await baseQuery.Where(t => t.Date >= currentMonthStart && t.Date <= currentMonthEnd)
                                                          .SumAsync(t => t.Amount);



            var categoryBreakdown = await baseQuery
                                    .GroupBy(t => t.Category.Name)
                                    .Select(g => new
                                    {
                                        CategoryName = g.Key,

                                        TotalAmountSpentCurrentMonth = g
                                        .Where(t => t.Date >= currentMonthStart && t.Date <= currentMonthEnd)
                                        .Sum(t => t.Amount),

                                        TotalAmountSpentPreviousMonth = g
                                        .Where(t => t.Date >= previousMonthStart && t.Date <= previousMonthEnd)
                                        .Sum(t => t.Amount)
                                    }).ToListAsync();

            var result = categoryBreakdown
                .Select(c => new CategoryBreakdownDto
                {
                    CategoryName = c.CategoryName,
                    TotalAmountSpentCurrentMonth = c.TotalAmountSpentCurrentMonth,
                    TotalAmountSpentPreviousMonth = c.TotalAmountSpentPreviousMonth,

                    PercentageOfTotal = (float)(currentMonthTotalExpense > 0 ?
                (c.TotalAmountSpentCurrentMonth / currentMonthTotalExpense) * 100 : 0),

                    PercentageChangeFromPreviousMonth = (float)(c.TotalAmountSpentPreviousMonth > 0 ?
                (c.TotalAmountSpentCurrentMonth - c.TotalAmountSpentPreviousMonth) / c.TotalAmountSpentPreviousMonth * 100 : 0)
                }).ToList(); 

            return result;
        }

        public AnalyticsInsightDto FindAnalyticsInsight()
        {
            var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

            var topCategory = _dbContext.Transactions
                                        .Include("Category")
                                        .Where(t => t.ApplicationUserId == _currentUserId &&
                                                    t.IsDeleted == false &&
                                                    t.Type == TransactionType.Expense &&
                                                    t.Date >= currentMonthStart &&
                                                    t.Date<= currentMonthEnd)
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
                                                        t.Date >= currentMonthStart &&
                                                        t.Date <= currentMonthEnd)
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
                CategoryWithHighestExpense = highestSpent ?? "Not found",
                AmountSpentInHighestCategory = amountSpentInHighestCategory ?? 0.00m,
                DateSpentMostOn = dateSpentMost ?? DateTime.MinValue,
                AmountSpentOnThatDay = amountSpentOnThatDay ?? 0
            };

            return analyticsInsight;
        }

        //Adding this method to check if a duplicate transaction exists before adding a new transaction
        public async Task<bool> IsDuplicateTransaction(Transaction transaction)
        {
            var isDuplicate = await _dbContext.Transactions.AnyAsync(t => t.ApplicationUserId == _currentUserId &&
                                                            t.IsDeleted == false &&
                                                            t.Amount == transaction.Amount &&
                                                            t.Date == transaction.Date &&
                                                            t.Type == transaction.Type &&
                                                            t.PaymentMode == transaction.PaymentMode &&
                                                            t.CategoryId == transaction.CategoryId);
            return isDuplicate;
        }

        public async Task<List<MonthlyExpenseTrendAnalyticsDto>> FindlyMonthlyExpenseTrend(int year)
        {
            var groupedMonthlyData = await _dbContext.Transactions
                                               .Where(
                                               t => t.ApplicationUserId == _currentUserId &&
                                                   t.IsDeleted == false &&
                                                   t.Type == TransactionType.Expense &&
                                                   t.Date.Year == year)
                                               .GroupBy(t => t.Date.Month)
                                               .Select(s => new MonthlyExpenseTrendAnalyticsDto
                                               {
                                                   Month = s.Key,
                                                   TotalExpense = s.Sum(t => t.Amount)
                                               })
                                               .ToDictionaryAsync(d => d.Month, d => d.TotalExpense);

            var monthlyExpenses = new List<MonthlyExpenseTrendAnalyticsDto>();

            for (int month = 1; month <= DateTime.Now.Month; month++)
            {
                
                if(groupedMonthlyData.TryGetValue(month, out decimal totalExpense))
                {
                    monthlyExpenses.Add(new MonthlyExpenseTrendAnalyticsDto
                    {
                        Month = month,
                        TotalExpense = totalExpense
                    });
                }
                else
                {
                    monthlyExpenses.Add(new MonthlyExpenseTrendAnalyticsDto
                    {
                        Month = month,
                        TotalExpense = 0
                    });
                }

            }

            return monthlyExpenses;
        }

        public HighestExpenseInfo FindLargestExpense(DateTime currentMonthStart, DateTime currentMonthEnd)
        {
            var largestExpenseInfo = _dbContext.Transactions
                                                    .Where(t => t.ApplicationUserId == _currentUserId &&
                                                             t.IsDeleted == false &&
                                                             t.Type == TransactionType.Expense &&
                                                             t.Date >= currentMonthStart &&
                                                             t.Date <= currentMonthEnd)
                                                    .OrderByDescending(t => t.Amount)
                                                    .Select(s => new HighestExpenseInfo
                                                    {
                                                        Amount = s.Amount,
                                                        Date = s.Date,
                                                        CategoryName = s.Category.Name
                                                    })
                                                    .FirstOrDefault();

            return largestExpenseInfo;
        }
    }
}
