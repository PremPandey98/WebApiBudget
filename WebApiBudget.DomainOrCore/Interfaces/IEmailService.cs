using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailVerificationAsync(string email, string otpCode);
        Task<bool> SendPasswordResetAsync(string email, string otpCode);
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}