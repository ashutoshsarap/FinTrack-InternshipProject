using FinTrack.Models;
using FinTrack.Models.Entity;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Service
{
    public class SendMonthlyReportService : ISendMonthlyReportService
    {

        private readonly IEmailSender _emailService;
        private readonly IAnalyticsService _analyticsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendMonthlyReportService(IEmailSender emailService, UserManager<ApplicationUser> userManager, IAnalyticsService analyticsService)
        {
            _emailService = emailService;
            _userManager = userManager;
            _analyticsService = analyticsService;
        }
        public async Task SendMonthlyReportEmailAsync()
        {
            var users = await _userManager.Users.Where(u => u.Email.Contains("@gmail.com")).ToListAsync();

            foreach (var user in users)
            {
                var email = user.Email;
                var subject = "Monthly Financial Report";
                var report = await _analyticsService.GetMonthlyReport(user.Id);
                var message = "Hello " + user.FullName + ",\n\nHere is your monthly financial report.";
                message += "\n";
                message += GenerateMonthlyReportContent(report);
                message += "\n\nBest regards,\nFinTrack Team";
                await _emailService.SendEmailAsync(email, subject, message);
            }

        }

        public string GenerateMonthlyReportContent(MonthlyReport report)
        {
            
            string content = $"Monthly Financial Report for {DateTime.Today.ToString("MMMM")} {DateTime.Today.Year}\n\n";
            content += $"\nTotal Income: {report.TotalIncome}\n";
            content += $"\nTotal Expense: {report.TotalExpense}\n";
            content += $"\nNet Savings: {report.NetSavings}\n\n";
            
            return content;

        }
    }

}
