using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiBudget.Application.Commonds;
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
            var users = await sender.Send(new Application.Queries.GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await sender.Send(new Application.Queries.GetUserByIdQuery(id));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

    }
}
