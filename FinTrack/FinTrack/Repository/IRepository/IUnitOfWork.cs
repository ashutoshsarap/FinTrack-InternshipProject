//V1
namespace FinTrack.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public TransactionRepository Transaction { get; }
        public CategoryRepository Category { get; }
        public ApplicationUserRepository ApplicationUser { get; }
        public Task Save();
    }
}
