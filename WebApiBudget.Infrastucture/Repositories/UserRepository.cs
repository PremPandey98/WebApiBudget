using Microsoft.EntityFrameworkCore;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class UserRepository(AppDbContext DbContext) : IUsersRepository
    {        public async Task<IEnumerable<UsersEntity>> GetAllUsersAsync()
        {
            return await DbContext.Users
                .Include(u => u.Groups)
                .ToListAsync();
        }

        public async Task<UsersEntity?> GetUsersByIdAsync(Guid userId)
        {
            return await DbContext.Users
                .Include(u => u.Groups)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<UsersEntity> AddUsersAsync(UsersEntity User)
        {
            User.UserId = Guid.NewGuid();
            DbContext.Users.Add(User);
            await DbContext.SaveChangesAsync();
            return User;
        }        
        public async Task<UsersEntity> UpdateUsersAsync(Guid userId, UsersEntity user)
        {
            // Load user with existing groups to properly manage the relationship
            var userToUpdate = await DbContext.Users
                .Include(u => u.Groups)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Only update properties that are provided (not null)
            if (user.Name != null) userToUpdate.Name = user.Name;
            if (user.UserName != null) userToUpdate.UserName = user.UserName;
            if (user.Email != null) userToUpdate.Email = user.Email;
            if (user.Password != null) userToUpdate.Password = user.Password;
            if (user.Phone != null) userToUpdate.Phone = user.Phone;
            if (user.IsActive != userToUpdate.IsActive) userToUpdate.IsActive = user.IsActive;
            
            // Always update the UpdatedAt timestamp
            userToUpdate.UpdatedAt = DateTime.UtcNow;

            // Handle groups update if provided
            if (user.Groups != null && user.Groups.Any())
            {
                // Get the IDs of the groups to be associated with the user
                var newGroupIds = user.Groups.Select(g => g.Id).ToList();

                // Fetch actual group entities from database
                var groupEntities = await DbContext.Groups
                    .Where(g => newGroupIds.Contains(g.Id))
                    .ToListAsync();

                if (groupEntities.Count == 0)
                {
                    throw new KeyNotFoundException("No valid groups found");
                }

                if (groupEntities.Count != newGroupIds.Count)
                {
                    // Some requested groups were not found
                    var foundGroupIds = groupEntities.Select(g => g.Id);
                    var missingGroupIds = newGroupIds.Except(foundGroupIds);
                    throw new KeyNotFoundException($"Groups not found: {string.Join(", ", missingGroupIds)}");
                }

                // Don't clear existing groups, just add new ones if they don't already exist
                foreach (var group in groupEntities)
                {
                    // Check if the group is already associated with the user
                    if (!userToUpdate.Groups.Any(g => g.Id == group.Id))
                    {
                        userToUpdate.Groups.Add(group);
                    }
                }
            }
            // Note: We no longer clear groups when not provided, maintaining existing associations

            // No need to call Update() explicitly when you've modified a tracked entity
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
            if(user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        public async Task<UsersEntity> UpdateUserGroupsAsync(Guid userId, List<Guid> groupIds, bool replaceExisting = false)
        {
            // Load user with existing groups
            var userToUpdate = await DbContext.Users
                .Include(u => u.Groups)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Fetch the specified groups from the database
            var groupsToAdd = await DbContext.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();

            if (groupsToAdd.Count == 0)
            {
                throw new KeyNotFoundException("No valid groups found");
            }

            // Check if all requested groups exist
            if (groupsToAdd.Count != groupIds.Count)
            {
                var foundGroupIds = groupsToAdd.Select(g => g.Id);
                var missingGroupIds = groupIds.Except(foundGroupIds);
                throw new KeyNotFoundException($"Groups not found: {string.Join(", ", missingGroupIds)}");
            }

            // If replacing existing groups
            if (replaceExisting)
            {
                userToUpdate.Groups.Clear();
            }

            // Add new groups
            foreach (var group in groupsToAdd)
            {
                if (!userToUpdate.Groups.Any(g => g.Id == group.Id))
                {
                    userToUpdate.Groups.Add(group);
                }
            }

            userToUpdate.UpdatedAt = DateTime.UtcNow;
            await DbContext.SaveChangesAsync();
            return userToUpdate;
        }
    }
}
