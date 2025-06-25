using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class ExpenseRecordsRepository : IExpenseRecordsRepository
    {
        private readonly AppDbContext _context;
        public ExpenseRecordsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ExpenseRecordsEntity> AddExpenseRecordAsync(ExpenseRecordsEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.ExpenseRecords.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<ExpenseRecordsEntity> UpdateExpenseRecordAsync(int expenseId, ExpenseRecordsEntity entity)
        {

            entity.UpdatedAt = DateTime.UtcNow;
            var ExpenseRecord = await _context.ExpenseRecords.FirstOrDefaultAsync(x => x.ExpenseId == expenseId);

            if (ExpenseRecord == null)
            {
                throw new KeyNotFoundException("Expense Record not found");
            }
            ExpenseRecord.Amount = entity.Amount;
            ExpenseRecord.Description = entity.Description;
            _context.ExpenseRecords.Update(ExpenseRecord);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<ExpenseRecordsEntity> GetExpenseRecordByIdAsync(int expenseId)
        {
            var ExpenseRecord = await _context.ExpenseRecords.FirstOrDefaultAsync(x => x.ExpenseId == expenseId);

            if (ExpenseRecord == null)
            {
                throw new KeyNotFoundException("Expense Record not found");
            }

            return ExpenseRecord;
        }
        public async Task<IEnumerable<ExpenseRecordsEntity>> GetAllExpenseRecordAsync()
        {
            return await _context.ExpenseRecords.ToListAsync();
        }
        public async Task<bool> DeleteExpenseRecordByIdAsync(int ExpenseId)
        {
            var ExpenseRecord = await _context.ExpenseRecords.FirstOrDefaultAsync(x => x.ExpenseId == ExpenseId);

            if (ExpenseRecord == null)
            {
                throw new KeyNotFoundException("Expense Record not foound");
            }
            ExpenseRecord.UpdatedAt = DateTime.UtcNow;
            ExpenseRecord.IsDeleted = true;
            _context.ExpenseRecords.Update(ExpenseRecord);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
