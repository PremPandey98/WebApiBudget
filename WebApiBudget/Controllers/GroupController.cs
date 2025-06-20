using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiBudget.Application.GroupCommandsOrQueries.Queries;
using WebApiBudget.Application.GroupCommandsOrQueries.Command;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;
        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup([FromBody] GroupEntity group)
        {
            if (group == null)
            {
                return BadRequest("Group cannot be null");
            }
            var result = await _sender.Send(new AddGroupCommand(group));
            return Ok(result?.ToDto());
        }
        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _sender.Send(new GetAllGroupsQuery());
            return Ok(groups?.ToDtos());
        }
        [HttpGet("GetGroup/{id}")]
        public async Task<IActionResult> GetGroupById(Guid id)
        {
            var group = await _sender.Send(new GetGroupByIdQuery(id));
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group.ToDto());
        }

        [HttpPost("UpdateGroup/{GroupId}")]
        public async Task<IActionResult> UpdateGroup(Guid GroupId, [FromBody] GroupEntity group)
        {
            if (group == null || group.Id != GroupId)
            {
                return BadRequest("Group cannot be null or ID mismatch");
            }
            var result = await _sender.Send(new UpdateGroupCommand(GroupId, group));
            return Ok(result?.ToDto());
        }

        [HttpPost("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var result = await _sender.Send(new DeleteGroupCommand(id));
            if (result)
            {
                return Ok(new { Success = true, Message = "Group deleted successfully" });
            }
            return NotFound(new { Success = false, Message = "Group not found" });
        }
    }
}
