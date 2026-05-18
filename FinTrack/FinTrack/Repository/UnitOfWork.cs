using FinTrack.Data;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Threading.Tasks;
//V1
namespace FinTrack.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ITransactionRepository Transaction { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IBudgetRepository Budget { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            Transaction = new TransactionRepository(context, currentUserService);
            Category = new CategoryRepository(context, currentUserService);
            Budget = new BudgetRepository(context, currentUserService);
            ApplicationUser = new IApplicationUserRepository(context, currentUserService);
        }
        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
}
