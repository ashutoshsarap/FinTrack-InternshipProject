using FinTrack.Models.Entity;
using Microsoft.EntityFrameworkCore;
using FinTrack.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//V1
namespace FinTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Transaction>().HasData(
                
                new Transaction
                {
                    Id = 1,
                    Amount = 100.00m,
                    Date = DateTime.Now,
                    Type = TransactionType.Income,
                    PaymentMode = PaymentMode.UPI,
                    Description = "Salary",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CategoryId = 1
                },
                new Transaction
                {
                    Id = 2,
                    Amount = 50.00m,
                    Date = DateTime.Now,
                    Type = Models.Enums.TransactionType.Expense,
                    PaymentMode = PaymentMode.UPI,
                    Description = "Bought Milk and bread",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CategoryId = 2
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Salary"
                },
                new Category
                {
                    Id = 2,
                    Name = "Groceries"
                }
            );
        }


    }
}
