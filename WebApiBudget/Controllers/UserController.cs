using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBudget.Application.UserCommandsOrQueries.Commonds;
using WebApiBudget.Application.UserCommandsOrQueries.Queries;
using WebApiBudget.DomainOrCore.Entities;

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
            return Ok(result);
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Policy = "RequireAdminRole")] // Only admins can see all users
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _sender.Send(new GetAllUsersQuery());
            return Ok(users);
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
            return Ok(user);
        }

        [HttpPost("UpdateUser/{UserId}")]
        [Authorize(Policy = "RequireAdminRole")] // Only admins can update users
        public async Task<IActionResult> UpdateUser([FromRoute] Guid UserId, [FromBody] UsersEntity User)
        {
            if (User == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = await _sender.Send(new UpdateUserCommand(UserId, User));
            return Ok(result);
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
            return Ok(user);
        }
    }
}
