using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Models.Enums;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly ApplicationDbContext _dbContext;

        public TransactionService(ITransactionRepository transactionRepository, ApplicationDbContext dbContext)
        {
            _transactionRepository = transactionRepository;
            _dbContext = dbContext;
        }

        public async Task CreateTransactionAsync(TransactionCreateDto transactionCreateDto)
        {
            if (transactionCreateDto == null)
            {
                throw new ArgumentNullException(nameof(transactionCreateDto));
            }
            if (transactionCreateDto.Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.");
            }
            if (transactionCreateDto.Date > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be in the future.");
            }
            
            if (!_dbContext.Categories.Any(c => c.Id == transactionCreateDto.CategoryId))
            {
                throw new ArgumentException("Category does not exist.");
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
                UpdatedAt = DateTime.Now
            };

            _transactionRepository.AddTransactionAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found.");
            }
            _transactionRepository.DeleteTransactionAsync(transaction.Result);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TransactionResponseDto>> GetAllTransactionsAsync()
        {
            
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

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

        public async Task<TransactionResponseDto> GetTransactionByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentException(nameof(filter));

                

            }

            var transaction = await _transactionRepository.GetTransactionByFilterAsync(filter);

            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found");
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

        public async Task<TransactionResponseDto> GetTransactionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid transaction ID.");
            }
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found.");
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

        public async Task<List<TransactionResponseDto>> GetTransactionsByFilterAsync(Expression<Func<Transaction, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }


            var transactions = await _transactionRepository.GetTransactionsByFilterAsync(filter);



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

        public async Task UpdateTransactionAsync(TransactionUpdateDto transactionUpdateDto)
        {
            if (transactionUpdateDto.Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.");
            }
            if (transactionUpdateDto.Date > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be in the future.");
            }

            if (!_dbContext.Categories.Any(c => c.Id == transactionUpdateDto.CategoryId))
            {
                throw new ArgumentException("Category does not exist.");
            }

            if(!Enum.IsDefined(typeof(PaymentMode), transactionUpdateDto.PaymentMode))
            {
                throw new ArgumentException("Given Payment mode does not exist");
            }

            if (!Enum.IsDefined(typeof(TransactionType), transactionUpdateDto.Type))
            {
                throw new ArgumentException("Given Type does not exist");
            }

            var transaction = new Transaction()
            {
                Id = transactionUpdateDto.Id,
                Amount = transactionUpdateDto.Amount,
                Description = transactionUpdateDto.Description,
                Date = transactionUpdateDto.Date,
                UpdatedAt = DateTime.Now,
                CategoryId = transactionUpdateDto.CategoryId,
                Type = transactionUpdateDto.Type,
                PaymentMode = transactionUpdateDto.PaymentMode
            };

            _transactionRepository.UpdateTransactionAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }


    }
}
