using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.AdminRepository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinTrack.Repository.AdminRepository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }


        public int FindTotalTransactionsCount()
        {
            return _dbContext.Transactions.Count();
        }

        public int FindTotalTransactionsCountForSpecificMonth(int currentMonth, int currentYear)
        {
            return _dbContext.Transactions.Count(t => t.Date.Month == currentMonth && t.Date.Year == currentYear);
        }

        public int FindTotalUsersCount()
        {
            return _userManager.Users.Count();
        }
    }
}
