using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Drawing.Text;

namespace FinTrack.Service
{
    public class RecurringTransactionJobService : IRecurringTransactionJobService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecurringTransactionJobService> _logger;

        public RecurringTransactionJobService(IUnitOfWork unitOfWork, ILogger<RecurringTransactionJobService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task ProcessTransaction(int recurringTransactionId)
        {
            var recurringTransaction = await _unitOfWork.RecurringTransaction.FindRecurringTransactionByIdAsync(recurringTransactionId);

            if(recurringTransaction == null)
            {
                return;
            }

            Transaction transaction = new Transaction
            {
                Amount = recurringTransaction.Amount,
                Description = recurringTransaction.Description,
                CategoryId = recurringTransaction.CategoryId,
                PaymentMode = recurringTransaction.PaymentMode,
                Type = recurringTransaction.TransactionType,
                Date = DateTime.Now,
                ApplicationUserId = recurringTransaction.ApplicationUserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            await _unitOfWork.Transaction.CreateAsync(transaction);

            recurringTransaction.NextExecutionDate = CalculateNextExecutionDate(recurringTransaction);

            await _unitOfWork.Save();
            _logger.LogInformation($"Processed recurring transaction with ID: {recurringTransactionId} and created transaction with ID: {transaction.Id}");
        }

        private DateTime CalculateNextExecutionDate(RecurringTransaction recurringTransaction)
        {
            DateTime nextExecutionDate = recurringTransaction.NextExecutionDate;
            switch (recurringTransaction.TransactionFrequency)
            {
                case TransactionFrequency.Daily:
                    nextExecutionDate = nextExecutionDate.AddDays(1);
                    break;
                case TransactionFrequency.Weekly:
                    nextExecutionDate = nextExecutionDate.AddDays(7);
                    break;
                case TransactionFrequency.Monthly:
                    nextExecutionDate = nextExecutionDate.AddMonths(1);
                    break;
                case TransactionFrequency.Annually:
                    nextExecutionDate = nextExecutionDate.AddYears(1);
                    break;
            }
            return nextExecutionDate;
        }

    }
}
