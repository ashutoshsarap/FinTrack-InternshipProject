namespace FinTrack.Service.IService
{
    public interface IRecurringTransactionJobService
    {
        Task ProcessTransaction(int recurringTransactionId);
    }
}
