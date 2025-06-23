using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;


namespace WebApiBudget.Application.GroupCommandsOrQueries.Validators
{
    public class AddGroupCommandValidator(IGroupRepository groupRepository )
    {
        public async Task<bool> ValidateAsync(GroupEntity group)
        {
            if (string.IsNullOrWhiteSpace(group.GroupCode))
            {
                throw new ArgumentException("Group code cannot be null or empty", nameof(group.GroupCode));
            }
            var existingGroup = await groupRepository.GetGroupByIdOrGroupCodeAsync(null,group.GroupCode);
            if (existingGroup != null)
            {
                throw new InvalidOperationException($"A group with the code '{group.GroupCode}' already exists.");
            }
            return true;
        }
    }
}
