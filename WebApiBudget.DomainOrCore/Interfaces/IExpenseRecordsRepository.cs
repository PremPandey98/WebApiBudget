using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IExpenseRecordsRepository
    {
        Task<ExpenseRecordsEntity> AddExpenseRecordAsync(ExpenseRecordsEntity entity);
        Task<ExpenseRecordsEntity> UpdateExpenseRecordAsync(int expenseId ,ExpenseRecordsEntity entity);
        Task<ExpenseRecordsEntity> GetExpenseRecordByIdAsync(int expenseId);
        Task<IEnumerable<ExpenseRecordsEntity>> GetAllExpenseRecordAsync();
        Task<bool> DeleteExpenseRecordByIdAsync(int expenseId);
    }
}
