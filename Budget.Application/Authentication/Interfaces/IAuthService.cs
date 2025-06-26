using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.Application.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateTokenAsync(UsersEntity user);
        Task<UsersEntity?> ValidateUserAsync(string username, string password);
        Task<GroupEntity?> GetGroupAsync(Guid GroupID);
        Task<UsersEntity?> GetUserAsync(Guid UserID);
        Task<string> GenerateTokenForGroupAsync(UsersEntity user, GroupEntity group);

    }
}
