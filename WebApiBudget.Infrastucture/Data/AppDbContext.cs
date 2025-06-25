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

    }
}
