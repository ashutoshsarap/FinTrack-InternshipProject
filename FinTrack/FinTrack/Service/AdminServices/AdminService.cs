using FinTrack.Models.AdminModelAndDtos.AdminDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.AdminRepository.Interfaces;
using FinTrack.Repository.IRepository;
using FinTrack.Service.AdminServices.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinTrack.Service.AdminServices
{
    public class AdminService : IAdminService
    {

        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public AdminDashboardDto GetAdminDashboardData()
        {

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            int totalUsers = _adminRepository.FindTotalUsersCount();
            int totalTransactions = _adminRepository.FindTotalTransactionsCount();
            int totalTransactionsThisMonth = _adminRepository.FindTotalTransactionsCountForSpecificMonth(currentMonth, currentYear);


            return new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                TotalTransactions = totalTransactions,
                TotalTransactionsThisMOnth = totalTransactionsThisMonth
            };

        }
    }
}
