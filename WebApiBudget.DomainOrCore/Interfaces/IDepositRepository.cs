using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IDepositRepository
    {
        Task<DepositEntity> AddDepositAsync(DepositEntity entity);
        Task<DepositEntity> UpdateDepositAsync(int depositId, DepositEntity entity);
        Task<DepositEntity> GetDepositByIdAsync(int depositId);
        Task<IEnumerable<DepositEntity>> GetAllDepositAsync();
        Task<IEnumerable<DepositEntity>> GetAllRelatedDepositsAsync(Guid user, Guid? group);
        Task<bool> DeleteDepositByIdAsync(int depositId);
    }
}