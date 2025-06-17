namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface ITokenBlacklistRepository
    {
        Task<bool> AddToBlacklistAsync(string token, Guid userId);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
