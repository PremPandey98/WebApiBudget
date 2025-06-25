using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.Infrastucture.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<BlacklistedTokenEntity> BlacklistedTokens { get; set; }
        public DbSet<ExpenseRecordsEntity> ExpenseRecords { get; set; }
        public DbSet<ExpenseCategoryEntity> ExpenseCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExpenseCategoryEntity>().HasData(
                new ExpenseCategoryEntity { ExpenseCategoryID = 1, ExpenseCategoryName = "Food" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 2, ExpenseCategoryName = "Hospital" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 3, ExpenseCategoryName = "Investment" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 4, ExpenseCategoryName = "Rent" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 5, ExpenseCategoryName = "Bill" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 6, ExpenseCategoryName = "Education" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 7, ExpenseCategoryName = "Transport" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 8, ExpenseCategoryName = "Entertainment" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 9, ExpenseCategoryName = "Utilities" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 10, ExpenseCategoryName = "Grocery" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 11, ExpenseCategoryName = "Travel" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 12, ExpenseCategoryName = "Insurance" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 13, ExpenseCategoryName = "Shopping" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 14, ExpenseCategoryName = "Loan" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 15, ExpenseCategoryName = "Miscellaneous" },
                new ExpenseCategoryEntity { ExpenseCategoryID = 16, ExpenseCategoryName = "creditCardBill" }
            );
        }
    }
}
