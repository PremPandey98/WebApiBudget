using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.DomainOrCore.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UsersEntity>> GetAllUsersAsync();
        Task<UsersEntity?> GetUsersByIdAsync(Guid userId);
        Task<UsersEntity> AddUsersAsync(UsersEntity User);
        Task<UsersEntity> UpdateUsersAsync(Guid userId, UsersEntity User);
        Task<bool> DeleteUsersByIdAsync(Guid userId);
        Task<UsersEntity?> GetUserByIdAsync(Guid userId);
        Task<UsersEntity> UpdateUserGroupsAsync(Guid userId, List<Guid> groupIds, bool replaceExisting = false);
    }
}
