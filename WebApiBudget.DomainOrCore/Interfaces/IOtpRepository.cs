using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IOtpRepository
    {
        Task<OtpEntity> CreateOtpAsync(OtpEntity otp);
        Task<OtpEntity?> GetValidOtpAsync(string email, string otpCode, OtpType type);
        Task<bool> MarkOtpAsUsedAsync(Guid otpId);
        Task<bool> InvalidateOtpsAsync(string email, OtpType type);
        Task<bool> CleanupExpiredOtpsAsync();
    }
}