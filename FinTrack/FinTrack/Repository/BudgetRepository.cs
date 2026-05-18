using FinTrack.Data;
using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Microsoft.EntityFrameworkCore;
//V1
namespace FinTrack.Repository
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _currentUserId;

        public BudgetRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _dbContext = context;
            _currentUserId = currentUserService.UserId;
        }
        public async Task CreateBudgetAsync(Budget budget)
        {
            await _dbContext.Budgets.AddAsync(budget);
        }

        public void DeleteBudgetAsync(Budget budget)
        {
            _dbContext.Budgets.Remove(budget);
        }

        public async Task<IEnumerable<BudgetResponseDto>> FindAllBudgetsByMonthAsync(int currentMonth, int currentYear)
        {
            var allBudgets = await _dbContext.Budgets
                                       .Where(b => b.ApplicationUserId == _currentUserId &&
                                              b.Month == currentMonth && 
                                              b.Year == currentYear)
                                       .Include(b => b.Category) // Include the related Category entity
                                       .Select(b => new BudgetResponseDto
                                       {
                                           Id = b.Id,
                                           CategoryId = b.CategoryId,
                                           MonthlyLimitAmount = b.MonthlyLimitAmount,
                                           CategoryName = b.Category.Name,
                                           Category = b.Category
                                       })
                                       .ToListAsync();
            return allBudgets;
        }

        public async Task<Budget> FindBudgetByIdAsync(int id)
        {
            Budget budget = await _dbContext.Budgets.Include(b => b.Category)
                                                    .FirstOrDefaultAsync(b => b.Id == id && 
                                                                         b.ApplicationUserId == _currentUserId);
            return budget;
        }

        public void UpdateBudgetAsync(Budget budget)
        {
            _dbContext.Update(budget);
        }

        // Check for duplicate budget 
        public async Task<bool> IsDuplicateBudgetAsync(Budget budget)
        {
            bool isDuplicate = await _dbContext.Budgets.AnyAsync(b => b.ApplicationUserId == _currentUserId &&
                                                                b.CategoryId == budget.CategoryId &&
                                                                b.Month == budget.Month &&
                                                                b.Year == budget.Year &&
                                                                b.Id != budget.Id);
            return isDuplicate;
        }

    }
}
