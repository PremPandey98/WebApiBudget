using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IExpenseCategoryRepository
    {
        Task<IEnumerable<ExpenseCategoryEntity>> GetAllAsync();
        Task<ExpenseCategoryEntity?> GetByIdAsync(int id);
        Task<ExpenseCategoryEntity> AddAsync(ExpenseCategoryEntity category);
        Task<ExpenseCategoryEntity> UpdateAsync(ExpenseCategoryEntity category);
        Task<bool> DeleteAsync(int id);
    }
}
