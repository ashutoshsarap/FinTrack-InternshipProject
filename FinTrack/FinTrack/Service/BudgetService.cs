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
        public BudgetService( IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserId = currentUserService.UserId;
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

            if(budgetCreateDto.CategoryId <= 0)
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
        }

        public async Task DeleteBudgetAsync(int budgetId)
        {
            Budget budgetToDelete = await _unitOfWork.Budget.FindBudgetByIdAsync(budgetId);
            if (budgetToDelete != null)
            {
                _unitOfWork.Budget.DeleteBudgetAsync(budgetToDelete);
                await _unitOfWork.Save();
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

        public async Task UpdateBudgetAsync(BudgetUpdateDto budgetUpdateDto)
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
            }            
        }
    }
}
