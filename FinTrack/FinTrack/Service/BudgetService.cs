using FinTrack.CustomExceptions;
using FinTrack.Models.DTOs.BudgetDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _currentUserId;
        private readonly ILogger<BudgetService> _logger;
        public BudgetService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<BudgetService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserId = currentUserService.UserId;
            _logger = logger;
        }

        public async Task CreateBudgetAsync(BudgetCreateDto budgetCreateDto)
        {

            if (budgetCreateDto == null)
            {
                throw new ArgumentNullException(nameof(budgetCreateDto), "Budget data cannot be null.");
            }

            if (budgetCreateDto.MonthlyLimitAmount <= 0)
            {
                throw new InvalidAmountException("Monthly limit amount must be greater than zero.");
            }

            if (budgetCreateDto.CategoryId <= 0)
            {
                throw new ArgumentException("Invalid category ID.", nameof(budgetCreateDto.CategoryId));
            }

            Budget budget = new Budget
            {
                CategoryId = budgetCreateDto.CategoryId,
                MonthlyLimitAmount = budgetCreateDto.MonthlyLimitAmount,
                ApplicationUserId = _currentUserId,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            if (await _unitOfWork.Budget.IsDuplicateBudgetAsync(budget))
            {
                throw new DuplicateRecordException("A budget for the specified category and month already exists.");
            }
            else
            {
                await _unitOfWork.Budget.CreateBudgetAsync(budget);
            }
            await _unitOfWork.Save();
            _logger.LogInformation("Budget with budget id : {Id} created successfully for user {UserId} with category {CategoryId} and amount {Amount}.", budget.Id,_currentUserId, budget.CategoryId, budget.MonthlyLimitAmount);
        }

        public async Task DeleteBudget(int budgetId)
        {
            Budget budgetToDelete = await _unitOfWork.Budget.FindBudgetByIdAsync(budgetId);
            if (budgetToDelete != null)
            {
                _unitOfWork.Budget.DeleteBudgetAsync(budgetToDelete);
                await _unitOfWork.Save();
                _logger.LogInformation("Budget with budget id : {Id} deleted successfully for user {UserId}.", budgetId, _currentUserId);
            }
            else
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");
            }
        }

        public async Task<IEnumerable<BudgetResponseDto>> GetAllBudgetsByMonthAsync(int month, int year)
        {
            var budgets = await _unitOfWork.Budget
                                           .FindAllBudgetsByMonthAsync(month, year);

            return budgets;
        }

        public async Task UpdateBudget(BudgetUpdateDto budgetUpdateDto)
        {
            if (budgetUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(budgetUpdateDto), "Budget data cannot be null.");
            }

            if (budgetUpdateDto.MonthlyLimitAmount <= 0)
            {
                throw new InvalidAmountException("Monthly limit amount must be greater than zero.");
            }

            Budget budgetToUpdate = await _unitOfWork.Budget.FindBudgetByIdAsync(budgetUpdateDto.Id);

            if (budgetToUpdate != null)
            {
                budgetToUpdate.MonthlyLimitAmount = budgetUpdateDto.MonthlyLimitAmount;
                budgetToUpdate.CategoryId = budgetUpdateDto.CategoryId;
                budgetToUpdate.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                throw new KeyNotFoundException($"Budget with ID {budgetUpdateDto.Id} not found.");
            }

            if (await _unitOfWork.Budget.IsDuplicateBudgetAsync(budgetToUpdate))
            {
                throw new DuplicateRecordException("A budget for the specified category and month already exists.");
            }
            else
            {
                await _unitOfWork.Save();
                _logger.LogInformation("Budget with budget id : {Id} updated successfully for user {UserId} with category {CategoryId} and amount {Amount}.", budgetToUpdate.Id, _currentUserId, budgetToUpdate.CategoryId, budgetToUpdate.MonthlyLimitAmount);
            }
        }

        public async Task<BudgetResponseDto> GetBudgetByIdAsync(int budgetId)
        {
            var budget = await _unitOfWork.Budget.FindBudgetByIdAsync(budgetId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");
            }
            return new BudgetResponseDto
            {
                Id = budget.Id,
                CategoryId = budget.CategoryId,
                MonthlyLimitAmount = budget.MonthlyLimitAmount,
                CategoryName = budget.Category.Name,
                Category = budget.Category
            };
        }

        public async Task<List<BudgetAnalyticsDto>> GetBudgetAnalytics()
        {
            var budgetAnalytics = await _unitOfWork.Budget.FindBudgetAnalytics();
            return budgetAnalytics;
        }
    }
}
