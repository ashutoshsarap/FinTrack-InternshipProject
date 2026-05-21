using FinTrack.Data;
using FinTrack.Models.DTOs.RecurringTransactionDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Repository
{
    public class RecurringTransactionRepository : IRecurringTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _currentUserId;
        public RecurringTransactionRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _dbContext = context;
            _currentUserId = currentUserService.UserId;
        }
        public async Task AddRecurringTransactionAsync(RecurringTransaction recurringTransaction)
        {
            await _dbContext.RecurringTransactions.AddAsync(recurringTransaction);
        }

        public void DeleteRecurringTransaction(RecurringTransaction recurringTransaction)
        {
            _dbContext.RecurringTransactions.Remove(recurringTransaction);
        }

        public async Task<IEnumerable<RecurringTransaction>> FindAllRecurringTransactionsAsync()
        {
            var allRecurringTransactions = await _dbContext.RecurringTransactions
                                                        .Where(rt => rt.ApplicationUserId == _currentUserId)    
                                                        .Include(rt => rt.Category)
                                                        .ToListAsync();
            return allRecurringTransactions;
        }

        public async Task<RecurringTransaction> FindRecurringTransactionByIdAsync(int id)
        {
            var recurringTransaction = await _dbContext.RecurringTransactions
                                            .Include(rt => rt.Category)
                                            .FirstOrDefaultAsync(rt => rt.Id == id && 
                                                                 rt.ApplicationUserId == _currentUserId);
            return recurringTransaction;
        }

        public void UpdateRecurringTransaction(RecurringTransaction recurringTransaction)
        {
            _dbContext.RecurringTransactions.Update(recurringTransaction);
        }

        //public async Task<IEnumerable<RecurringTransaction>> FindAllPendingRecurringTransactionsForJobAsync()
        //{
        //    var allRecurringTransactions = await _dbContext.RecurringTransactions
        //                                                .Where(rt => rt.NextExecutionDate <= DateTime.Now)
        //                                                .Include(rt => rt.Category)
        //                                                .ToListAsync();
        //    return allRecurringTransactions;
        //}
    }
}
