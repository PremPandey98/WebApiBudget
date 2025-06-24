using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Models.DTOs;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UsersEntity>> GetAllUsersAsync();
        Task<UsersEntity?> GetUserByIdOrUserNameAsync(Guid? userId,string? usreName);
        Task<UsersEntity> AddUsersAsync(UsersEntity User);
        Task<UsersEntity> UpdateUsersAsync(Guid userId, UserDto User);
        Task<bool> DeleteUsersByIdAsync(Guid userId);
        Task<UsersEntity> UpdateUserGroupsAsync(Guid userId, GroupEntity groupDto);
        Task<UsersEntity> RemoveUserGroupAsync(Guid userId, Guid groupId);
    }
}
