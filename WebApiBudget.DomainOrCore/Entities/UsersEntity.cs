using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBudget.DomainOrCore.Entities
{
    public class UsersEntity
    {
        [Key]
        public Guid UserId { get; set; }

        public string Name { get; set; } = null!;

        public string UserName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public int Role { get; set; } = (int)UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public List<GroupEntity> Groups { get; set; } = new List<GroupEntity>();
    }
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
}
