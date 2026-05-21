using FinTrack.CustomExceptions;
using FinTrack.Models.DTOs.RecurringTransactionDtos;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Hangfire;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class RecurringTransactionService : IRecurringTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _currentUserId;
        public RecurringTransactionService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserId = currentUserService.UserId;
        }

        public async Task CreateRecurringTransactionAsync(RecurringTransactionCreateDto recurringTransactionCreateDto)
        {
            if (recurringTransactionCreateDto == null)
            {
                throw new ArgumentNullException(nameof(recurringTransactionCreateDto));
            }
            if (recurringTransactionCreateDto.Amount <= 0)
            {
                throw new InvalidAmountException("Amount must be greater than 0");
            }
            //if (recurringTransactionCreateDto.StartDate < DateTime.Today)
            //{
            //    throw new Exception("Starting Date cannot be in the past");
            //}

            string jobId = Guid.NewGuid().ToString();

            RecurringTransaction recurringTransaction = new RecurringTransaction
            {
                Amount = recurringTransactionCreateDto.Amount,
                StartDate = recurringTransactionCreateDto.StartDate,
                Description = recurringTransactionCreateDto.Description,
                PaymentMode = recurringTransactionCreateDto.PaymentMode,
                TransactionType = recurringTransactionCreateDto.TransactionType,
                TransactionFrequency = recurringTransactionCreateDto.TransactionFrequency,
                CategoryId = recurringTransactionCreateDto.CategoryId,
                ApplicationUserId = _currentUserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                NextExecutionDate = recurringTransactionCreateDto.StartDate,
                HangFireId = jobId
            };

            await _unitOfWork.RecurringTransaction.AddRecurringTransactionAsync(recurringTransaction);
            await _unitOfWork.Save();

            string cronExpression = CalculateCronExpression(recurringTransaction);

            RecurringJob.AddOrUpdate<IRecurringTransactionJobService>(
                jobId,
                x => x.ProcessTransaction(recurringTransaction.Id),
                cronExpression
                );

            if(recurringTransactionCreateDto.StartDate == DateTime.Today)
            {
                BackgroundJob.Enqueue<IRecurringTransactionJobService>(x => x.ProcessTransaction(recurringTransaction.Id));
            }

        }

        public async Task DeleteRecurringTransaction(int id)
        {
            RecurringTransaction transactionToBeDeleted = await _unitOfWork.RecurringTransaction.FindUserSpecificRecurringTransactionByIdAsync(id);

            if (transactionToBeDeleted == null)
            {
                throw new RecordNotFoundException("Record not found");
            }

            _unitOfWork.RecurringTransaction.DeleteRecurringTransaction(transactionToBeDeleted);
            RecurringJob.RemoveIfExists(transactionToBeDeleted.HangFireId);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<RecurringTransactionResponseDto>> GetAllRecurringTransactionsAsync()
        {
            var allRecurringTransactions = await _unitOfWork.RecurringTransaction.FindAllRecurringTransactionsAsync();

            return allRecurringTransactions.Select(rt => new RecurringTransactionResponseDto
            {
                Id = rt.Id,
                Amount = rt.Amount,
                Description = rt.Description,
                StartDate = rt.StartDate,
                TransactionFrequency = rt.TransactionFrequency,
                TransactionType = rt.TransactionType,
                PaymentMode = rt.PaymentMode,
                CategoryId = rt.CategoryId,
                CategoryName = rt.Category.Name,
                Category = rt.Category,
                NextExecutionDate = rt.NextExecutionDate
            });

        }

        public async Task<RecurringTransactionResponseDto> GetRecurringTransactionByIdAsync(int id)
        {
            RecurringTransaction recurringTransaction = await _unitOfWork.RecurringTransaction.FindUserSpecificRecurringTransactionByIdAsync(id);

            return new RecurringTransactionResponseDto
            {
                Id = recurringTransaction.Id,
                Amount = recurringTransaction.Amount,
                Description = recurringTransaction.Description,
                TransactionFrequency = recurringTransaction.TransactionFrequency,
                PaymentMode = recurringTransaction.PaymentMode,
                TransactionType = recurringTransaction.TransactionType,
                CategoryId = recurringTransaction.CategoryId,
                CategoryName = recurringTransaction.Category.Name,
                Category = recurringTransaction.Category,
                NextExecutionDate = recurringTransaction.NextExecutionDate,
                StartDate=recurringTransaction.StartDate,
                HangFireId = recurringTransaction.HangFireId
            };
        }

        public async Task UpdateRecurringTransaction(RecurringTransactionUpdateDto recurringTransactionUpdateDto)
        {
            if (recurringTransactionUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(recurringTransactionUpdateDto));
            }

            if (recurringTransactionUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(recurringTransactionUpdateDto));
            }
            if (recurringTransactionUpdateDto.Amount <= 0)
            {
                throw new InvalidAmountException("Amount must be greater than 0");
            }
            if (recurringTransactionUpdateDto.StartDate < DateTime.Today)
            {
                throw new Exception("Starting Date cannot be in the past");
            }

            var recurringTransactionToUpdate = await _unitOfWork.RecurringTransaction.FindUserSpecificRecurringTransactionByIdAsync(recurringTransactionUpdateDto.Id);

            recurringTransactionToUpdate.Amount = recurringTransactionUpdateDto?.Amount ?? 0;
            recurringTransactionToUpdate.Description = recurringTransactionUpdateDto.Description;
            recurringTransactionToUpdate.TransactionFrequency = recurringTransactionUpdateDto.TransactionFrequency;
            recurringTransactionToUpdate.TransactionType = recurringTransactionUpdateDto.TransactionType;
            recurringTransactionToUpdate.PaymentMode = recurringTransactionUpdateDto.PaymentMode;
            recurringTransactionToUpdate.UpdatedAt = DateTime.Now;
            recurringTransactionToUpdate.CategoryId = recurringTransactionUpdateDto.CategoryId;
            recurringTransactionToUpdate.StartDate = recurringTransactionUpdateDto.StartDate;
            recurringTransactionToUpdate.NextExecutionDate = recurringTransactionUpdateDto.TransactionFrequency switch
            {
                TransactionFrequency.Daily => DateTime.Today.AddDays(1),
                TransactionFrequency.Weekly => DateTime.Today.AddDays(7),
                TransactionFrequency.Monthly => DateTime.Today.AddMonths(1),
                TransactionFrequency.Annually => DateTime.Today.AddYears(1),
                _ => recurringTransactionToUpdate.NextExecutionDate
            };

            await _unitOfWork.Save();
            RecurringJob.AddOrUpdate<IRecurringTransactionJobService>(
                recurringTransactionToUpdate.HangFireId,
                x => x.ProcessTransaction(recurringTransactionToUpdate.Id),
                CalculateCronExpression(recurringTransactionToUpdate)
                );

            if (recurringTransactionToUpdate.StartDate == DateTime.Today)
            {
                BackgroundJob.Enqueue<IRecurringTransactionJobService>(x => x.ProcessTransaction(recurringTransactionToUpdate.Id));
            }
        }

        //public async Task<IEnumerable<RecurringTransactionResponseDto>> GetAllPendingRecurringTransactionsAsync()
        //{
        //    var allRecurringTransactions = await _unitOfWork.RecurringTransaction.FindAllPendingRecurringTransactionsForJobAsync();



        //    return allRecurringTransactions.Select(rt => new RecurringTransactionResponseDto
        //    {
        //        Id = rt.Id,
        //        Amount = rt.Amount,
        //        Description = rt.Description,
        //        TransactionFrequency = rt.TransactionFrequency,
        //        PaymentMode = rt.PaymentMode,
        //        TransactionType = rt.TransactionType,
        //        CategoryId = rt.CategoryId,
        //        CategoryName = rt.Category.Name,
        //        Category = rt.Category,
        //        NextExecutionDate=rt.NextExecutionDate
        //    });
        //}

        private string CalculateCronExpression(RecurringTransaction transaction)
        {
            string cronExpression = string.Empty;
            switch (transaction.TransactionFrequency)
            {
                case TransactionFrequency.Daily:
                    cronExpression = $"0 0 * * *"; // Every day at midnight
                    break;
                case TransactionFrequency.Weekly:
                    cronExpression = $"0 0 * * {((int)transaction.StartDate.DayOfWeek)}"; // Every week on the same day as StartDate
                    break;
                case TransactionFrequency.Monthly:
                    cronExpression = $"0 0 {transaction.StartDate.Day} * *"; // Every month on the same day as StartDate
                    break;
                case TransactionFrequency.Annually:
                    cronExpression = $"0 0 {transaction.StartDate.Day} {transaction.StartDate.Month} *"; // Every year on the same day and month as StartDate
                    break;
            }
            return cronExpression;
        }

    }
}
