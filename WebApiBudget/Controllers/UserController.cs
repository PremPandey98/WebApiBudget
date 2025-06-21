using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using WebApiBudget.Application.UserCommandsOrQueries.Commonds;
using WebApiBudget.Application.UserCommandsOrQueries.Queries;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Helpers;
using WebApiBudget.DomainOrCore.Models.DTOs;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all actions
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("AddUser")]
        [AllowAnonymous] // any one can add 
        public async Task<IActionResult> AddUser([FromBody] UsersEntity user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = await _sender.Send(new AddUserCommand(user));
            return Ok(result?.ToDto());
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Policy = "RequireAdminRole")] // Only admins can see all users
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _sender.Send(new GetAllUsersQuery());
            return Ok(users?.ToDtos());
        }

        [HttpGet("GetUser/{id}")]
        [Authorize(Policy = "RequireUserRole")] // Both users and admins can see user details
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _sender.Send(new GetUserByIdQuery(id));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToDto());
        }

        [HttpPost("UpdateUser/{UserId}")]
        [Authorize(Policy = "RequireUserRole")] // Only admins can update users
        public async Task<IActionResult> UpdateUser([FromRoute] Guid UserId, [FromBody] UsersEntity User)
        {
            if (User == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = await _sender.Send(new UpdateUserCommand(UserId, User));
            return Ok(result?.ToDto());
        }

        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Policy = "RequireAdminRole")] // Only admins can delete users
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var result = await _sender.Send(new DeleteUserCommand(id));
            if (!result)
            {
                return NotFound("User not found or could not be deleted");
            }
            return Ok(result);
        }
        
        [HttpGet("me")]
        [Authorize] // Any authenticated user can access their own profile
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized();
            }

            var user = await _sender.Send(new GetUserByIdQuery(parsedUserId));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToDto());
        }

        [HttpPost("UpdateUserGroups/{userId}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> UpdateUserGroups(Guid userId, [FromBody] UserGroupsUpdateDto groupsUpdateDto)
        {
            if (groupsUpdateDto == null || groupsUpdateDto.GroupIds == null)
            {
                return BadRequest("Group IDs must be provided");
            }
            
            try
            {
                // Access the repository directly since we're adding a specialized method
                var userRepository = HttpContext.RequestServices.GetRequiredService<IUsersRepository>();
                var user = await userRepository.UpdateUserGroupsAsync(
                    userId, 
                    groupsUpdateDto.GroupIds, 
                    groupsUpdateDto.ReplaceExisting);
                    
                return Ok(user.ToDto());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("UpdateUserPartial/{userId}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> UpdateUserPartial([FromRoute] Guid userId, [FromBody] UserUpdateDto updateDto)
        {
            try
            {
                // First get the existing user
                var existingUser = await _sender.Send(new GetUserByIdQuery(userId));
                if (existingUser == null)
                {
                    return NotFound("User not found");
                }
                
                // Only update the fields that are provided
                if (updateDto.Name != null) existingUser.Name = updateDto.Name;
                if (updateDto.UserName != null) existingUser.UserName = updateDto.UserName;
                if (updateDto.Email != null) existingUser.Email = updateDto.Email;
                if (updateDto.Password != null) existingUser.Password = updateDto.Password;
                if (updateDto.Phone != null) existingUser.Phone = updateDto.Phone;
                if (updateDto.IsActive.HasValue) existingUser.IsActive = updateDto.IsActive.Value;
                
                // Handle groups separately if provided
                if (updateDto.Groups != null && updateDto.Groups.Any())
                {
                    // Only modify the Groups property - leave other properties unchanged
                    var groupIds = updateDto.Groups.Select(g => g.Id).ToList();
                    var userRepository = HttpContext.RequestServices.GetRequiredService<IUsersRepository>();
                    var updatedUser = await userRepository.UpdateUserGroupsAsync(userId, groupIds, false);
                    return Ok(updatedUser.ToDto());
                }
                
                // Submit the update if we haven't returned yet
                var result = await _sender.Send(new UpdateUserCommand(userId, existingUser));
                return Ok(result?.ToDto());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
