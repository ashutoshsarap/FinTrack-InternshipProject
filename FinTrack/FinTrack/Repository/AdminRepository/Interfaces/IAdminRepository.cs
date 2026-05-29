namespace FinTrack.Repository.AdminRepository.Interfaces
{
    public interface IAdminRepository
    {
        public int FindTotalUsersCount();
        public int FindTotalTransactionsCount();
        public int FindTotalTransactionsCountForSpecificMonth(int currentMonth, int currentYear);
    }
}
