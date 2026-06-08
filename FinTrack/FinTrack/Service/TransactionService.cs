using FinTrack.Data;
using FinTrack.Models.Enums;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FinTrack.Models.ViewModels;
using FinTrack.CustomExceptions;
using FinTrack.Models.DTOs.TransactionDto;
using FinTrack.Models.Pagination;
//V4
namespace FinTrack.Service
{
    public class TransactionService : ITransactionService
    {

        //private readonly DummyITransactionRepository _transactionRepository;
        //private readonly ApplicationDbContext _dbContext;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionService> _logger;
        private readonly IAuditService _auditService;
        public TransactionService(IUnitOfWork unitOfWork, ILogger<TransactionService> logger, IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _auditService = auditService;
        }

        public async Task CreateTransactionAsync(string userId, string userName,TransactionCreateDto transactionCreateDto)
        {
            if (transactionCreateDto == null)
            {
                throw new ArgumentNullException(nameof(transactionCreateDto));
            }
            if (transactionCreateDto.Amount <= 0)
            {
                throw new InvalidAmountException("Amount must be greater than zero.");
            }
            if (transactionCreateDto.Date > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be in the future.");
            }
            
            var transaction = new Transaction
            {
                Amount = transactionCreateDto.Amount,
                Date = transactionCreateDto.Date,
                Type = transactionCreateDto.Type,
                PaymentMode = transactionCreateDto.PaymentMode,
                Description = transactionCreateDto.Description,
                CategoryId = transactionCreateDto.CategoryId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                ApplicationUserId = userId
            };

            if (!await _unitOfWork.Transaction.IsDuplicateTransaction(transaction))
            {
                await _unitOfWork.Transaction.CreateAsync(transaction);
                _logger.LogInformation("Transaction created successfully for user {UserId} with transaction ID {Id}", userId, transaction.Id);
            }
            else
            {
                throw new DuplicateRecordException("Duplicate transaction found");
            }
            
            await _unitOfWork.Save();

            AuditData auditData = new AuditData
            {
                UserName = userName,
                Action = "Create",
                EntityActedUpon = "Transaction",
                EntityId = transaction.Id,
                Timestamp = DateTime.UtcNow
            };
            await _auditService.LogAuditDataAsync(auditData);
        }

        public async Task DeleteTransaction(int id,string userName, string userId)
        {
            var transaction = await _unitOfWork.Transaction.FindAsync(id, null);
            if (transaction == null)
            {
                throw new RecordNotFoundException("Transaction not found."); 
            }
            _unitOfWork.Transaction.Delete(transaction);
            await _unitOfWork.Save();
            _logger.LogInformation("Transaction with ID {Id} deleted successfully for user {UserId}", id, userId);
            AuditData auditData = new AuditData
            {
                UserName = userName,
                Action = "Delete",
                EntityActedUpon = "Transaction",
                EntityId = transaction.Id,
                Timestamp = DateTime.UtcNow
            };
            await _auditService.LogAuditDataAsync(auditData);
        }

        public async Task<List<TransactionResponseDto>> GetAllTransactionsAsync()
        {
            
            var transactions = await _unitOfWork.Transaction.FindAllAsync(includeProperties: "Category");
            
            return transactions.Where(t => t.IsDeleted==false).Select(t => new TransactionResponseDto()
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Type = t.Type,
                PaymentMode = t.PaymentMode,
                Description = t.Description,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name
            }).ToList();
        }

        public async Task<TransactionResponseDto> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentException(nameof(filter));
            }

            var transaction = await _unitOfWork.Transaction.FindTransactionByFilterAsync(filter, includeProperties: "Category");

            if (transaction == null)
            {
                throw new RecordNotFoundException("Transaction not found with the specified filter.");
            }

            var transactionResponse = new TransactionResponseDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type,
                PaymentMode = transaction.PaymentMode,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name
            };

            return transactionResponse;

        }

        public async Task<TransactionResponseDto> GetTransactionByIdAsync(int id, string userId, string includeProperties)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid transaction ID.");
            }
            var transaction = await _unitOfWork.Transaction.FindAsync(id, includeProperties: "Category");
            if (transaction == null)
            {
                throw new RecordNotFoundException("Transaction not found.");
            }
            var transactionResponse = new TransactionResponseDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type,
                PaymentMode = transaction.PaymentMode,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name
            };

            return transactionResponse;
        }

        public async Task<TransactionResponsePage> GetAllTransactionsByFilterAsync(TransactionFilterDto filterDto)
        {
            if (filterDto == null)
            {
                throw new ArgumentNullException(nameof(filterDto));
            }


            var paginatedTransactions = await _unitOfWork.Transaction.FindAllTransactionByFilterAsync(filterDto, includeProperties: "Category");

            var transactionResponses = paginatedTransactions.AllTransactions.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Type = t.Type,
                PaymentMode = t.PaymentMode,
                Description = t.Description,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name
            }).ToList();

            return new TransactionResponsePage
            {
                Transactions = transactionResponses,
                TotalCountOfTransactions = paginatedTransactions.TotalCountOfTransactions,
                PageSize = paginatedTransactions.PageSize,
                CurrentPageNumber = paginatedTransactions.CurrentPageNumber
            };
        }

        public async Task UpdateTransaction(int id, string userName, string userId,TransactionUpdateDto transactionUpdateDto)
        {

            if (transactionUpdateDto.Amount <= 0)
            {
                throw new InvalidAmountException("Amount must be greater than zero.");
            }
            if (transactionUpdateDto.Date > DateTime.Now)
            {
                throw new InvalidDataException("Date cannot be in the future.");
            }

            var transaction = await _unitOfWork.Transaction.FindAsync(id,includeProperties: null);
            if (transaction == null)
            {
                throw new RecordNotFoundException("Transaction not found.");
            }

            Transaction checkTransaction = new Transaction
            {
                Id = id,
                Amount = transactionUpdateDto.Amount,
                Date = transactionUpdateDto.Date,
                Type = transactionUpdateDto.Type,
                PaymentMode = transactionUpdateDto.PaymentMode,
                Description = transactionUpdateDto.Description,
                CategoryId = transactionUpdateDto.CategoryId,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = DateTime.Now,
                ApplicationUserId = userId
            };

            if (!await _unitOfWork.Transaction.IsDuplicateTransaction(checkTransaction))
            {
                transaction.Amount = transactionUpdateDto.Amount;
                transaction.Date = transactionUpdateDto.Date;
                transaction.Type = transactionUpdateDto.Type;
                transaction.PaymentMode = transactionUpdateDto.PaymentMode;
                transaction.Description = transactionUpdateDto.Description;
                transaction.CategoryId = transactionUpdateDto.CategoryId;
                transaction.UpdatedAt = DateTime.Now;
            }
            else
            {
                throw new DuplicateRecordException("Duplicate transaction found");
            }


            await _unitOfWork.Save();
            _logger.LogInformation("Transaction with ID {Id} updated successfully for user {UserId}", id, userId);
            AuditData auditData = new AuditData
            {
                UserName = userName,
                Action = "Update",
                EntityActedUpon = "Transaction",
                EntityId = transaction.Id,
                Timestamp = DateTime.UtcNow
            };
            await _auditService.LogAuditDataAsync(auditData);
        }

        public async Task<DashboardViewModel> GetDashboardData()
        {

            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;

            var totalIncomeCurrentMonth = _unitOfWork.Transaction.FindTotalAmountByTypeAndMonth(TransactionType.Income, currentMonth, currentYear);
            var totalIncomePreviousMonth = _unitOfWork.Transaction.FindTotalAmountByTypeAndMonth(TransactionType.Income, previousMonth, currentYear);

            var totalExpensesCurrentMonth = _unitOfWork.Transaction.FindTotalAmountByTypeAndMonth(TransactionType.Expense, currentMonth, currentYear);
            var totalExpensePreviousMonth = _unitOfWork.Transaction.FindTotalAmountByTypeAndMonth(TransactionType.Expense, previousMonth, currentYear);

            var netBalance = totalIncomeCurrentMonth - totalExpensesCurrentMonth;
            var recentTransactions = await _unitOfWork.Transaction.FindRecentTransactions();
            var expenseCategorySummaries = await _unitOfWork.Transaction.FindCategoryWiseExpense();
            var savingsRate = Math.Round(totalIncomeCurrentMonth > 0 ? (totalIncomeCurrentMonth - totalExpensesCurrentMonth) / totalIncomeCurrentMonth * 100 : 0, 2);

            float expensePercentageChange = (float) Math.Round(totalExpensePreviousMonth > 0 ? ((totalExpensesCurrentMonth - totalExpensePreviousMonth) / totalExpensePreviousMonth) * 100 : 0);

            float incomePercentageChange = (float) Math.Round(totalIncomePreviousMonth > 0 ? ((totalIncomeCurrentMonth - totalIncomePreviousMonth) / totalIncomePreviousMonth) * 100 : 0);

            var dashboardData = new DashboardViewModel
            {
                TotalIncome = totalIncomeCurrentMonth,
                TotalExpenses = totalExpensesCurrentMonth,
                IncomePercentageChange = incomePercentageChange,
                ExpensePercentageChange = expensePercentageChange,
                NetBalance = netBalance,
                SavingsRate = (float)savingsRate,
                RecentTransactions = recentTransactions.Select(t => new TransactionResponseDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Type = t.Type,
                    PaymentMode = t.PaymentMode,
                    Description = t.Description,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name
                }).ToList(),

                ExpenseCategorySummaries = expenseCategorySummaries
            };

            return dashboardData;
        }

        public async Task<List<TransactionResponseDto>> SearchTransactionsAsync(string userId, string term)
        {
            return await _unitOfWork.Transaction.SearchAsync(userId, term);
        }


    }
}
