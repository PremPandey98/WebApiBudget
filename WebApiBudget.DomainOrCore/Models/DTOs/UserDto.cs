using System;
using System.Collections.Generic;

namespace WebApiBudget.DomainOrCore.Models.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
    }
}
