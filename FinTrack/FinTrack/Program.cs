using FinTrack.Data;
using FinTrack.Models;
using FinTrack.Models.Entity;
using FinTrack.Repository;
using FinTrack.Repository.IRepository;
using FinTrack.Service;
using FinTrack.Service.IService;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Hangfire configuration for recurring transactions
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

//Binds the EmailSettings section of the configuration to the EmailSettings class, allowing us to inject IOptions<EmailSettings> into our services to access email configuration values.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddHangfireServer();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

//To access the current HTTP context and retrieve user information, we need to register the IHttpContextAccessor service, which allows us to access the HttpContext in our services.
builder.Services.AddHttpContextAccessor();

//Repository registration
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();  
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddScoped<ISendMonthlyReportService, SendMonthlyReportService>();

var app = builder.Build();

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

app.MapRazorPages();

app.UseHangfireDashboard();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

RecurringJob.AddOrUpdate<ISendMonthlyReportService>("monthly-report",s => s.SendMonthlyReportEmailAsync(), "0 8 1 * *");

app.Run();
