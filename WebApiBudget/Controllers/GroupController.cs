using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiBudget.Application.GroupCommandsOrQueries.Queries;
using WebApiBudget.Application.GroupCommandsOrQueries.Command;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebApiBudget.DomainOrCore.Models.DTOs;
using WebApiBudget.Infrastucture.Services;
using System.Security.Claims;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly ISender _sender;
        public GroupController(ISender sender)
        {
            _sender = sender;
        }

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

        [HttpGet("GetGroupUsers/{groupId}")]
        public async Task<IActionResult> GetGroupUsers(Guid groupId)
        {
            var group = await _sender.Send(new GetGroupByIdQuery(groupId));
            if (group == null)
            {
                return NotFound("Group not found");
            }
            // Return user details as DTOs
            return Ok(group.Users != null ? group.Users.ToDtos() : new List<UserDto>());
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

        [HttpPost("JoinGroup")]
        public async Task<IActionResult> JoinGroup([FromBody] JoinGroupRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized();
            }

            // For now, use existing GetGroupByIdQuery and implement basic join logic
            // You'll need to create these commands/queries later
            var groups = await _sender.Send(new GetAllGroupsQuery());
            var group = groups?.FirstOrDefault(g => g.GroupCode == request.GroupCode);
            
            if (group == null)
            {
                return NotFound("Group not found");
            }

            if (group.Password != request.Password)
            {
                return BadRequest("Invalid group password");
            }

            // Check if user is already in the group
            if (group.Users.Any(u => u.UserId == parsedUserId))
            {
                return BadRequest("User is already a member of this group");
            }

            // For now, return success message - you'll need to implement AddUserToGroupCommand
            var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Someone";
            var title = "New Group Member";
            var body = $"{userName} wants to join the group!";
            

            return Ok(new { Message = "Join request processed (you'll need to implement AddUserToGroupCommand)", GroupId = group.Id, GroupName = group.GroupName });
        }
    }

    public class JoinGroupRequest
    {
        public string GroupCode { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
