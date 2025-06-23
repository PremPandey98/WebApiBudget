using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models.DTOs;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class UserRepository(AppDbContext DbContext) : IUsersRepository
    {
        public async Task<IEnumerable<UsersEntity>> GetAllUsersAsync()
        {
            return await DbContext.Users
                .Include(u => u.Groups)
                .ToListAsync();
        }

        public async Task<UsersEntity?> GetUserByIdOrUserNameAsync(Guid? userId, string? userName)
        {
            if (userId == null && string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Either userId or userName must be provided");
            }
            return await DbContext.Users.FirstOrDefaultAsync(x => (userId != null && x.UserId == userId) || (!string.IsNullOrEmpty(userName) && x.UserName == userName));
        }
        public async Task<UsersEntity> AddUsersAsync(UsersEntity User)
        {
            User.UserId = Guid.NewGuid();
            User.CreatedAt = DateTime.UtcNow;
            User.UpdatedAt = DateTime.UtcNow;
            User.IsActive = true;
            User.Role = (int)UserRole.User;
            DbContext.Users.Add(User);
            await DbContext.SaveChangesAsync();
            return User;
        }
        public async Task<UsersEntity> UpdateUsersAsync(Guid userId, UserDto user)
        {

            var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (user.Name != null && user.Name != userToUpdate.Name) userToUpdate.Name = user.Name;
            if (user.UserName != null && user.UserName != userToUpdate.UserName) userToUpdate.UserName = user.UserName;
            if (user.Email != null && user.Email != userToUpdate.Email) userToUpdate.Email = user.Email;
            if (user.Password != null && user.Password != userToUpdate.Password) userToUpdate.Password = user.Password;
            if (user.Phone != null && user.Phone != userToUpdate.Phone) userToUpdate.Phone = user.Phone;

            userToUpdate.UpdatedAt = DateTime.UtcNow;
            DbContext.Update(userToUpdate);
            await DbContext.SaveChangesAsync();
            return userToUpdate;
        }
        public async Task<bool> DeleteUsersByIdAsync(Guid userId)
        {
            var DeactivateUser = await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (DeactivateUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            DeactivateUser.IsActive = false;
            DeactivateUser.UpdatedAt = DateTime.UtcNow;
            DbContext.Users.Update(DeactivateUser);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public async Task<UsersEntity?> GetUserByIdAsync(Guid userId)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        public async Task<UsersEntity> UpdateUserGroupsAsync(Guid userId, GroupEntity group)
        {
            var userToUpdate = await DbContext.Users.Include(u => u.Groups).FirstOrDefaultAsync(x => x.UserId == userId);

            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (userToUpdate.Groups.Any(g => g.Id == group.Id))
            {
                throw new InvalidOperationException("Group is already added to the user.");
            }
            userToUpdate.Groups.Add(group);

            DbContext.Users.Update(userToUpdate);
            await DbContext.SaveChangesAsync();
            return userToUpdate;
        }
    }
}
