using FinTrack.Models.Entity;
using Microsoft.EntityFrameworkCore;
using FinTrack.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FinTrack.Service.IService;
//V2
namespace FinTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        //private readonly ICurrentUserService _currentUserService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //_currentUserService = currentUserService;
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public DbSet<AuditData> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //All the following were added to prevent cascading delete when an ApplicationUser is deleted, which would otherwise delete all related Transactions and Categories.
            //Transaction and ApplicationUser relationship configuration

            modelBuilder.Entity<Transaction>() // Configure the relationship between Transaction and ApplicationUser
                        .HasOne(t => t.ApplicationUser) // Each Transaction has one ApplicationUser
                        .WithMany() // An ApplicationUser can have many Transactions
                        .HasForeignKey(t => t.ApplicationUserId) // Tells EF Core that the foreign key is ApplicationUserId
                        .OnDelete(DeleteBehavior.NoAction); // Prevents cascading delete when an ApplicationUser is deleted

            //Category and Transaction relationship configuration
            modelBuilder.Entity<Transaction>() 
                        .HasOne(t => t.Category) 
                        .WithMany()
                        .HasForeignKey(t => t.CategoryId)
                        .OnDelete(DeleteBehavior.NoAction);

            //Category and ApplicationUser relationship configuration
            modelBuilder.Entity<Category>()
                        .HasOne(c => c.ApplicationUser)
                        .WithMany()
                        .HasForeignKey(c => c.ApplicationUserId)
                        .OnDelete(DeleteBehavior.NoAction);

            // Global query filter to exclude soft-deleted transactions
            //Any query that retrieves transactions will automatically exclude those where IsDeleted is true, ensuring that soft-deleted transactions are not returned in query results.
            //modelBuilder.Entity<Transaction>()
            //            .HasQueryFilter(t => !t.IsDeleted && t.ApplicationUserId==_currentUserService.UserId); 

            //modelBuilder.Entity<Category>()
            //            .HasQueryFilter(c => c.ApplicationUserId == _currentUserService.UserId || c.IsSystemDefined); // Include system-defined categories for all users

            //modelBuilder.Entity<Transaction>().HasData(

            //    new Transaction
            //    {
            //        Id = 1,
            //        Amount = 100.00m,
            //        Date = DateTime.Now,
            //        Type = TransactionType.Income,
            //        PaymentMode = PaymentMode.UPI,
            //        Description = "Salary",
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now,
            //        CategoryId = 1,
            //        IsDeleted = false,
            //        DeletedAt = null,
            //        ApplicationUserId = "1bb3d59b-3ca2-4cdc-b1b1-0f82b2adc7f5"
            //    },
            //    new Transaction
            //    {
            //        Id = 2,
            //        Amount = 50.00m,
            //        Date = DateTime.Now,
            //        Type = Models.Enums.TransactionType.Expense,
            //        PaymentMode = PaymentMode.UPI,
            //        Description = "Bought Milk and bread",
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now,
            //        CategoryId = 2,
            //        IsDeleted = false,
            //        DeletedAt = null,
            //        ApplicationUserId = "1bb3d59b-3ca2-4cdc-b1b1-0f82b2adc7f5"
            //    }
            //);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Salary",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 2,
                    Name = "Groceries",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 3,
                    Name = "Entertainment",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 4,
                    Name = "Food and Dining",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 5,
                    Name = "Transaport",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 6,
                    Name = "Shopping",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 7,
                    Name = "Medical",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 8,
                    Name = "Education",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 9,
                    Name = "Bills",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                },
                new Category
                {
                    Id = 10,
                    Name = "Transfer",
                    ApplicationUserId = null,
                    IsSystemDefined = true
                }
            );
        }


    }
}
