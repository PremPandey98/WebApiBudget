using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly AppDbContext _context;
        public ExpenseCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpenseCategoryEntity>> GetAllAsync()
        {
            return await _context.ExpenseCategories.ToListAsync();
        }

        public async Task<ExpenseCategoryEntity?> GetByIdAsync(int id)
        {
            return await _context.ExpenseCategories.FindAsync(id);
        }

        public async Task<ExpenseCategoryEntity> AddAsync(ExpenseCategoryEntity category)
        {
            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ExpenseCategoryEntity> UpdateAsync(ExpenseCategoryEntity category)
        {
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ExpenseCategories.FindAsync(id);
            if (entity == null) return false;
            _context.ExpenseCategories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
