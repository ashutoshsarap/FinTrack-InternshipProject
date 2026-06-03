using FinTrack.Data;
using FinTrack.Handler;
using FinTrack.HangFireAuth;
using FinTrack.Middlewares;
using FinTrack.Models;
using FinTrack.Models.Entity;
using FinTrack.Repository;
using FinTrack.Repository.AdminRepository;
using FinTrack.Repository.AdminRepository.Interfaces;
using FinTrack.Repository.IRepository;
using FinTrack.Service;
using FinTrack.Service.AdminServices;
using FinTrack.Service.AdminServices.Interfaces;
using FinTrack.Service.IService;
using FinTrack.Utilities;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using Serilog;


Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Integrate Serilog as the logging provider for the application, allowing us to log structured and detailed information about the application's behavior and performance to both the console and rolling log files.
builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Hangfire configuration for recurring transactions
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

//Binds the EmailSettings section of the configuration to the EmailSettings class, allowing us to inject IOptions<EmailSettings> into our services to access email configuration values.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddHangfireServer();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

//To access the current HTTP context and retrieve user information, we need to register the IHttpContextAccessor service, which allows us to access the HttpContext in our services.
builder.Services.AddHttpContextAccessor();

//Repository registration
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();  
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Service registration
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<ICsvExportService, CsvExportService>();
builder.Services.AddScoped<ICsvImportService, CsvImportService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();
builder.Services.AddScoped<IRecurringTransactionJobService, RecurringTransactionJobService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISendMonthlyReportService, SendMonthlyReportService>();
builder.Services.AddScoped<IGeneratePdfService, GeneratePdfService>();
builder.Services.AddScoped<IAdminService, AdminService>();




var app = builder.Build();


//One time seeding script to assign existing users a role of user
//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//    var users = await userManager.Users.ToListAsync();

//    foreach(var user in users)
//    {
//        if (!await userManager.IsInRoleAsync(user, Roles.User) &&
//            !await userManager.IsInRoleAsync(user, Roles.Admin))
//        {
//            await userManager.AddToRoleAsync(user, "User");
//        }
//    }
//}

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//    foreach(var role in new[] { "Admin", "User" })
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }

//    var adminUser = await userManager.FindByEmailAsync("ashutoshsarapgdsc@gmail.com");
//    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
//    {
//        await userManager.AddToRoleAsync(adminUser, "Admin");
//    }
//}

//app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseExceptionHandler("/Home/Error");

//app.UseMiddleware<CustomLoggingMiddleware>();
//app.UseMiddleware<AuditMiddleware>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//Adding authorization filter to the Hangfire dashboard to restrict access to users with the "Admin" role. This ensures that only authorized users can view and manage the background jobs in the Hangfire dashboard.
app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions
    {
        Authorization = new[] { new HangFireAuthorizationByRole() } // Restrict access to the dashboard to users with the "Admin" role
    });

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=RedirectUser}/{id?}");

// Schedule the recurring job to send monthly report emails on the 1st of every month at 8 AM using Hangfire's Cron expression.
RecurringJob.AddOrUpdate<ISendMonthlyReportService>("monthly-report", s => s.SendMonthlyReportEmailAsync(), "0 8 1 * *");
//RecurringJob.AddOrUpdate<ISendMonthlyReportService>("monthly-report",s => s.SendMonthlyReportEmailAsync(), Cron.Minutely);

app.Run();
Log.Information("Application started successfully."); //for testing