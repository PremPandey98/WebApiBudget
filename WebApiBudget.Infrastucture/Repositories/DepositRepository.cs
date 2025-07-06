using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly AppDbContext _context;
        public DepositRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<DepositEntity> AddDepositAsync(DepositEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Deposits.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public async Task<DepositEntity> UpdateDepositAsync(int depositId, DepositEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            var deposit = await _context.Deposits.FirstOrDefaultAsync(x => x.DepositId == depositId);

            if (deposit == null)
            {
                throw new KeyNotFoundException("Deposit not found");
            }
            deposit.Amount = entity.Amount;
            deposit.Description = entity.Description;
            deposit.Tittle = entity.Tittle;
            deposit.UpdatedAt = entity.UpdatedAt;
            _context.Deposits.Update(deposit);
            await _context.SaveChangesAsync();
            return deposit;
        }
        
        public async Task<DepositEntity> GetDepositByIdAsync(int depositId)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(x => x.DepositId == depositId);

            if (deposit == null)
            {
                throw new KeyNotFoundException("Deposit not found");
            }

            return deposit;
        }
        
        public async Task<IEnumerable<DepositEntity>> GetAllDepositAsync()
        {
            return await _context.Deposits.Where(x => x.IsDeleted == false).ToListAsync();
        }
        
        public async Task<bool> DeleteDepositByIdAsync(int depositId)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(x => x.DepositId == depositId);

            if (deposit == null)
            {
                throw new KeyNotFoundException("Deposit not found");
            }
            deposit.UpdatedAt = DateTime.UtcNow;
            deposit.IsDeleted = true;
            _context.Deposits.Update(deposit);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<DepositEntity>> GetAllRelatedDepositsAsync(Guid user, Guid? group)
        {
            if (!group.HasValue || group.Value == Guid.Empty)
            {
                return await _context.Deposits
                    .Where(x => x.AddedByUserId == user && x.IsGroupRelated == false && x.IsDeleted == false)
                    .Include(x => x.AddedByUser)
                    .ToListAsync();
            }
            else
            {
                return await _context.Deposits
                    .Where(x => x.GroupId == group && x.IsGroupRelated == true && x.IsDeleted == false)
                    .Include(x => x.AddedByUser)
                    .Include(x => x.Group)
                    .ToListAsync();
            }
        }
    }
}