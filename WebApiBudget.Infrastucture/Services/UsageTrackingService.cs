using WebApiBudget.Infrastucture.Data;
using WebApiBudget.DomainOrCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApiBudget.Infrastucture.Services
{
    public class UsageTrackingService
    {
        private readonly AppDbContext _context;

        public UsageTrackingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CheckHighUsageAsync(Guid userId, decimal thresholdAmount = 1000m)
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var monthlyExpenses = await _context.ExpenseRecords
                .Where(e => e.AddedByUserId == userId 
                           && e.CreatedAt.Month == currentMonth 
                           && e.CreatedAt.Year == currentYear
                           && !e.IsDeleted)
                .SumAsync(e => e.Amount);

            //if (monthlyExpenses >= thresholdAmount)
            //{
            //    await _notificationService.NotifyHighUsageAsync(userId, monthlyExpenses, "monthly");
            //}
        }

        public async Task CheckGroupHighUsageAsync(Guid groupId, decimal thresholdAmount = 5000m)
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var groupExpenses = await _context.ExpenseRecords
                .Where(e => e.GroupId == groupId 
                           && e.CreatedAt.Month == currentMonth 
                           && e.CreatedAt.Year == currentYear
                           && !e.IsDeleted)
                .SumAsync(e => e.Amount);

            //if (groupExpenses >= thresholdAmount)
            //{
            //    var title = "Group High Usage Alert";
            //    var body = $"Your group's monthly spending has reached ${groupExpenses:F2}. Time to review the budget!";
            //    await _notificationService.NotifyGroupMembersAsync(groupId, title, body);
            //}
        }
    }
}