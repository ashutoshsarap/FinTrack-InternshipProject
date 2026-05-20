using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.Enums;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FinTrack.Models.ViewModels;
using FinTrack.CustomExceptions;
//V3
namespace FinTrack.Service
{
    public class TransactionService : ITransactionService
    {

        //private readonly DummyITransactionRepository _transactionRepository;
        //private readonly ApplicationDbContext _dbContext;

        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task CreateTransactionAsync(string userId,TransactionCreateDto transactionCreateDto)
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
            }
            else
            {
                throw new DuplicateRecordException("Duplicate transaction found");
            }
            
            await _unitOfWork.Save();
        }

        public async Task DeleteTransaction(int id, string userId)
        {
            var transaction = await _unitOfWork.Transaction.FindAsync(id, null);
            if (transaction == null)
            {
                throw new RecordNotFoundException("Transaction not found."); 
            }
            _unitOfWork.Transaction.Delete(transaction);
            await _unitOfWork.Save();
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

        public async Task<List<TransactionResponseDto>> GetAllTransactionsByFilterAsync(TransactionFilterDto filterDto)
        {
            if (filterDto == null)
            {
                throw new ArgumentNullException(nameof(filterDto));
            }


            var transactions = await _unitOfWork.Transaction.FindAllTransactionByFilterAsync(filterDto, includeProperties: "Category");

            return transactions.Select(t => new TransactionResponseDto()
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

        public async Task UpdateTransaction(int id, string userId,TransactionUpdateDto transactionUpdateDto)
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
        }

        public async Task<DashboardViewModel> GetDashboardData()
        {
            var totalIncome = _unitOfWork.Transaction.FindTotalAmountByType(TransactionType.Income);
            var totalExpenses = _unitOfWork.Transaction.FindTotalAmountByType(TransactionType.Expense);
            var netBalance = totalIncome - totalExpenses;
            var recentTransactions = await _unitOfWork.Transaction.FindRecentTransactions();
            var expenseCategorySummaries = await _unitOfWork.Transaction.FindCategoryWiseExpense();

            var dashboardData = new DashboardViewModel
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                NetBalance = netBalance,
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

    }
}
