using System;

namespace WebApiBudget.DomainOrCore.Models.DTOs
{
    public class GroupDto
    {
        public Guid Id { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCode { get; set; }
        public string? Description { get; set; }
        public string? Password { get; set; } 
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Intentionally omitting Users to break the circular reference
    }
}
