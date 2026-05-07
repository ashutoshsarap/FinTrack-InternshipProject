//V1
namespace FinTrack.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public TransactionRepository Transaction { get; }
        public Task Save();
    }
}
