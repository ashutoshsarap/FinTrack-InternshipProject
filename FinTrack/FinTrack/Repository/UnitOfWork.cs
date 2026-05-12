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
        public TransactionRepository Transaction { get; private set; }
        public CategoryRepository Category { get; private set; }

        public ApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            Transaction = new TransactionRepository(context, currentUserService);
            Category = new CategoryRepository(context, currentUserService);
            ApplicationUser = new ApplicationUserRepository(context, currentUserService);
        }
        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
}
