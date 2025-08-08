using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiBudget.DomainOrCore.Entities
{
    public class OtpEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Email { get; set; } = null!;
        
        public string OtpCode { get; set; } = null!;
        
        public OtpType Type { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime ExpiresAt { get; set; }
        
        public bool IsUsed { get; set; } = false;
        
        public DateTime? UsedAt { get; set; }
    }
    
    public enum OtpType
    {
        EmailVerification = 0,
        PasswordReset = 1
    }
}