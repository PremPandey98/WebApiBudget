using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBudget.DomainOrCore.Entities
{
    public class GroupEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string GroupName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string GroupCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public List<UsersEntity> Users { get; set; } = new List<UsersEntity>();
    }
}
