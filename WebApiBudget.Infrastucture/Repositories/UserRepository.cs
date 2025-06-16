using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class UserRepository(AppDbContext DbContext) : IUsersRepository
    {
        public async Task<IEnumerable<UsersEntity>> GetAllUsersAsync()
        {
            return await DbContext.Users.ToListAsync();
        }

        public async Task<UsersEntity?> GetUsersByIdAsync(Guid userId)
        {
            return await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<UsersEntity> AddUsersAsync(UsersEntity User)
        {
            User.UserId = Guid.NewGuid();
            DbContext.Users.Add(User);
            await DbContext.SaveChangesAsync();
            return User;
        }
        public async Task<UsersEntity> UpdateUsersAsync(Guid userId, UsersEntity User)
        {
            var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            userToUpdate.Name = User.Name;
            userToUpdate.UserName = User.UserName;
            userToUpdate.Email = User.Email;
            userToUpdate.Password = User.Password;
            userToUpdate.Phone = User.Phone;
            userToUpdate.UpdatedAt = DateTime.UtcNow;
            userToUpdate.IsActive = User.IsActive;
            DbContext.Users.Update(userToUpdate);
            await DbContext.SaveChangesAsync();
            return User;
        }
        public async Task<bool> DeleteUsersByIdAsync(Guid userId)
        {
            var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            userToUpdate.IsActive = false;
            userToUpdate.UpdatedAt = DateTime.UtcNow;
            DbContext.Users.Update(userToUpdate);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public async Task<UsersEntity?> GetUserByIdAsync(Guid userId)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if(user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }
    }
}
