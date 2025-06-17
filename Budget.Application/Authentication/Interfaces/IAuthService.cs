using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.Application.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateTokenAsync(UsersEntity user);
        Task<UsersEntity?> ValidateUserAsync(string username, string password);
    }
}
