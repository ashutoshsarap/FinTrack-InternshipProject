using FinTrack.Data;
using FinTrack.Models.DTOs;
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
            return await query.Where(t => t.ApplicationUserId==_currentUserId && t.IsDeleted==false)
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


        public decimal GetTotalAmountByType(TransactionType type)
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var totalAmount = _dbContext.Transactions.Where(t => t.ApplicationUserId==_currentUserId && 
                                                             t.IsDeleted==false && 
                                                             t.Type == type &&
                                                             t.Date>=startDate &&
                                                             t.Date<=endDate)
                                                      .Sum(t => t.Amount);
            return totalAmount;
        }

        public async Task<List<Transaction>> GetRecentTransactions()
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var recentTransactions = await _dbContext.Transactions
                                                     .Include("Category")
                                                     .Where(t => t.ApplicationUserId==_currentUserId &&
                                                            t.IsDeleted==false &&
                                                            t.Date >= startDate &&
                                                            t.Date <= endDate)
                                                     .OrderByDescending(t => t.Date)
                                                     .Take(5)
                                                     .ToListAsync();
            return recentTransactions;
        }

        public async Task<List<CategoryExpenseDto>> GetCategoryWiseExpense()
        {
            var (startDate, endDate) = GetCurrentMonthDateRange();
            var categoryWiseExpense = await _dbContext.Transactions
                                                .Include("Category")
                                                .Where(t => t.ApplicationUserId==_currentUserId && 
                                                            t.IsDeleted==false &&
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
    }
}
