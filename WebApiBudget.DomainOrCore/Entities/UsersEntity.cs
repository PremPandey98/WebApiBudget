using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Models.DTOs;

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
        
        // Removed email verification fields for now since we're using cache-based approach
        // You can add these back later when you apply the migration:
        // public bool IsEmailVerified { get; set; } = false;
        // public string? EmailVerificationToken { get; set; }
        // public DateTime? EmailVerificationTokenExpiry { get; set; }
        // public string? PasswordResetToken { get; set; }
        // public DateTime? PasswordResetTokenExpiry { get; set; }
        
        public List<GroupEntity> Groups { get; set; } = new List<GroupEntity>();
    }
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
}
