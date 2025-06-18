using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiBudget.Application.GroupCommandsOrQueries.Command;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.Helpers;
using WebApiBudget.DomainOrCore.Models.DTOs;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup([FromBody] GroupEntity group)
        {
            if (group == null)
            {
                return BadRequest("Group cannot be null");
            }
            var result = await _sender.Send(new AddGroupCommand(group));
            return Ok(result?.ToDto());
        }
    }
}
