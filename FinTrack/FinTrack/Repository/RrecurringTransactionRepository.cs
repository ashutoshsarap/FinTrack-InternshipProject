using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Repository
{
    public class RrecurringTransactionRepository : IRecurringTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _currentUserId;
        public RrecurringTransactionRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _dbContext = context;
            _currentUserId = currentUserService.UserId;
        }
        public async Task AddRecurringTransactionAsync(RecurringTransaction recurringTransaction)
        {
            await _dbContext.RecurringTransactions.AddAsync(recurringTransaction);
        }

        public void DeleteRecurringTransactionAsync(RecurringTransaction recurringTransaction)
        {
            _dbContext.RecurringTransactions.Remove(recurringTransaction);
        }

        public async Task<IEnumerable<RecurringTransaction>> GetAllRecurringTransactionsAsync()
        {
            var allRecurringTransactions = await _dbContext.RecurringTransactions
                                                        .Where(rt => rt.ApplicationUserId == _currentUserId)
                                                        .Include(rt => rt.Category)
                                                        .ToListAsync();
            return allRecurringTransactions;
        }

        public async Task<RecurringTransaction> GetRecurringTransactionByIdAsync(int id)
        {
            var recurringTransaction = await _dbContext.RecurringTransactions
                                            .Include(rt => rt.Category)
                                            .FirstOrDefaultAsync(rt => rt.Id == id && 
                                                                 rt.ApplicationUserId == _currentUserId);
            return recurringTransaction;
        }

        public void UpdateRecurringTransactionAsync(RecurringTransaction recurringTransaction)
        {
            _dbContext.RecurringTransactions.Update(recurringTransaction);
        }
    }
}
