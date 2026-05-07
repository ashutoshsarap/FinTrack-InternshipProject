using FinTrack.Data;
using FinTrack.Repository.IRepository;
using System.Threading.Tasks;
//V1
namespace FinTrack.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository Transaction { get; private set; }
        public CategoryRepository Category { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Transaction = new TransactionRepository(context);
            Category = new CategoryRepository(context);
        }
        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
}
