//V1
namespace FinTrack.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ITransactionRepository Transaction { get; }
        public ICategoryRepository Category { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public Task Save();
    }
}
