using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models.DTOs;

namespace WebApiBudget.Application.UserCommandsOrQueries.Validators
{
    public class UpdateUserGroupCommandValidator(IGroupRepository groupRepository)
    {
        public async Task<GroupEntity> ValidateAsync(GroupDto groupDto)
        { 
            var group = await groupRepository.GetGroupByIdOrGroupCodeAsync(null, groupDto.GroupCode);

            if (group == null)
                throw new KeyNotFoundException("Invalid Group code or Group password.");
            else
            {
                if (!group.IsActive)
                    throw new InvalidOperationException("The group is not active.");
                if (group.Password != groupDto.Password)
                    throw new InvalidOperationException("Invalid group password.");
            }
            return group;
        }

    }
}
