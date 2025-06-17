using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiBudget.Application.UserCommandsOrQueries.Commonds;
using WebApiBudget.Application.UserCommandsOrQueries.Queries;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    public class UserController(ISender sender) : ControllerBase
    {
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UsersEntity user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = await sender.Send(new AddUserCommand(user));
            return Ok(result);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await sender.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await sender.Send(new GetUserByIdQuery(id));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("UpdateUser/{UserId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid UserId, [FromBody] UsersEntity User)
        {
            if (User == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = await sender.Send(new UpdateUserCommand(UserId, User));
            return Ok(result);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var result = await sender.Send(new DeleteUserCommand(id));
            if (!result)
            {
                return NotFound("User not found or could not be deleted");
            }
            return Ok(result);
        }
    }
}
