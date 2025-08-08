using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string email, OtpType type);
        Task<bool> ValidateOtpAsync(string email, string otpCode, OtpType type);
        Task<bool> InvalidateOtpAsync(string email, OtpType type);
        string GenerateRandomOtp(int length = 6);
    }
}