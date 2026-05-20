using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;

namespace FinTrack.Service
{
    public class RecurringTransactionJobService : IRecurringTransactionJobService
    {

        private readonly IUnitOfWork _unitOfWork;

        public RecurringTransactionJobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ProcessTransactions()
        {
            var pendingTransactions = await _unitOfWork.RecurringTransaction.FindAllPendingRecurringTransactionsForJobAsync();

            foreach (var recurringTransaction in pendingTransactions)
            {
                Transaction transactionEntity = new Transaction
                {
                    Amount = recurringTransaction.Amount,
                    CategoryId = recurringTransaction.CategoryId,
                    Description = recurringTransaction.Description,
                    PaymentMode = recurringTransaction.PaymentMode,
                    Type = recurringTransaction.TransactionType,
                    Date = DateTime.Now,
                    ApplicationUserId = recurringTransaction.ApplicationUserId,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.Transaction.CreateAsync(transactionEntity);
            }
            await _unitOfWork.Save();
        }
    }
}
