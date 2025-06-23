using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<GroupEntity>> GetAllGroupsAsync();
        Task<GroupEntity?> GetGroupByIdOrGroupCodeAsync(Guid? groupId,string? GroupCode);
        Task<GroupEntity> AddGroupAsync(GroupEntity group);
        Task<GroupEntity> UpdateGroupAsync(Guid groupId, GroupEntity group);
        Task<bool> DeleteGroupByIdAsync(Guid groupId);
        Task<IEnumerable<GroupEntity>> GetGroupsByUserIdAsync(Guid userId);
        Task<bool> IsGroupNameUniqueAsync(string groupName, Guid? excludeGroupId = null);
    }
}
