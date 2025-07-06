using System;
using System.Collections.Generic;
using System.Linq;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Models.DTOs;

namespace WebApiBudget.Helpers
{
    public static class MappingExtensions
    {
        public static UserDto ToDto(this UsersEntity entity)
        {
            if (entity == null)
                return null;

            return new UserDto
            {
                UserId = entity.UserId,
                Name = entity.Name,
                UserName = entity.UserName,
                Email = entity.Email,
                Phone = entity.Phone,
                Role = entity.Role,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Groups = entity.Groups?.Select(g => g.ToDto()).ToList() ?? new List<GroupDto>()
            };
        }

        public static GroupDto ToDto(this GroupEntity entity)
        {
            if (entity == null)
                return null;

            return new GroupDto
            {
                Id = entity.Id,
                GroupName = entity.GroupName,
                GroupCode = entity.GroupCode,
                Description = entity.Description,
                Password = entity.Password,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static List<UserDto> ToDtos(this IEnumerable<UsersEntity> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<UserDto>();
        }

        public static List<GroupDto> ToDtos(this IEnumerable<GroupEntity> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<GroupDto>();
        }
    }
}
