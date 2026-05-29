using FinTrack.Models.AdminModelAndDtos;
using FinTrack.Models.AdminModelAndDtos.AdminDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.AdminRepository.Interfaces;
using FinTrack.Repository.IRepository;
using FinTrack.Service.AdminServices.Interfaces;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace FinTrack.Service.AdminServices
{
    public class AdminService : IAdminService
    {

        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public AdminService(IAdminRepository adminRepository, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
            _emailService = emailService;
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

        public async Task CreateAdmin(CreateAdminDto createAdminDto)
        {
            ApplicationUser newAdmin = new ApplicationUser
            {
                FullName =  createAdminDto.FullName,
                UserName = createAdminDto.Email,
                Email = createAdminDto.Email,
                EmailConfirmed = true
            };

            if (await _userManager.FindByEmailAsync(createAdminDto.Email) != null)
            {
                throw new Exception("A user with this email already exists");
            }

            var result = await _userManager.CreateAsync(newAdmin, createAdminDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newAdmin, "Admin");

                var subject = "Admin Account Created";
                var message = $"Hello {newAdmin.FullName},\n\nYour admin account has been successfully created. You can now log in using your Email: {newAdmin.Email}.\n Password : {createAdminDto.Password} \nBest regards,\nFinTrack Team";
                await _emailService.SendEmailAsync(newAdmin.Email, subject, message);
            }
            else
            {
                throw new Exception("Failed to create admin user");
            }

        }
    }
}
