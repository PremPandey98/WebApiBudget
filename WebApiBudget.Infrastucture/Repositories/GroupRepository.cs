using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class GroupRepository(AppDbContext DbContext) : IGroupRepository
    {        
        public async Task<IEnumerable<GroupEntity>> GetAllGroupsAsync()
        {
            return await DbContext.Groups
                .Include(g => g.Users)
                .ToListAsync();
        }
        public async Task<GroupEntity?> GetGroupByIdOrGroupCodeAsync(Guid? groupId, string? GroupCode)
        {
            return await DbContext.Groups.FirstOrDefaultAsync(x => (groupId != null && x.Id == groupId) || (!string.IsNullOrEmpty(GroupCode) && x.GroupCode == GroupCode));
        }
        public async Task<GroupEntity> AddGroupAsync(GroupEntity group)
        {
            group.CreatedAt = DateTime.UtcNow;
            group.Id = Guid.NewGuid();
            DbContext.Groups.Add(group);
            await DbContext.SaveChangesAsync();
            return group;
        }
        public async Task<GroupEntity> UpdateGroupAsync(Guid groupId, GroupEntity group)
        {
            var groupToUpdate = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (groupToUpdate == null)
            {
                throw new KeyNotFoundException("Group not found");
            }
            groupToUpdate.GroupName = group.GroupName;
            groupToUpdate.Description = group.Description;
            groupToUpdate.GroupCode = group.GroupCode;
            groupToUpdate.UpdatedAt = DateTime.UtcNow;
            DbContext.Groups.Update(groupToUpdate);
            await DbContext.SaveChangesAsync();
            return group;
        }
        public async Task<bool> DeleteGroupByIdAsync(Guid groupId)
        {
            var groupToDelete = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (groupToDelete == null)
            {
                throw new KeyNotFoundException("Group not found");
            }
            groupToDelete.UpdatedAt = DateTime.UtcNow;
            groupToDelete.IsActive = false;
            DbContext.Groups.Update(groupToDelete);
            return await DbContext.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<GroupEntity>> GetGroupsByUserIdAsync(Guid userId)
        {
            return await DbContext.Groups.Where(g => g.Id == userId).ToListAsync();
        }
        public async Task<bool> IsGroupNameUniqueAsync(string groupName, Guid? excludeGroupId = null)
        {
            return !await DbContext.Groups.AnyAsync(g => g.GroupName == groupName && g.Id != excludeGroupId);
        }
    }
}
