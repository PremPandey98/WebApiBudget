using System;
using System.Collections.Generic;

namespace WebApiBudget.DomainOrCore.Models.DTOs
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; }
        public List<GroupReferenceDto>? Groups { get; set; }
    }

    public class GroupReferenceDto
    {
        public Guid Id { get; set; }
    }
}
