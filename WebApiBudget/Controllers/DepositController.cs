using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Application.Deposits.Commands;
using Budget.Application.Deposits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApiBudget.DomainOrCore.Entities;
using System.Security.Claims;

namespace WebApiBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepositController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DepositController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllDeposit")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllDepositsQuery());
            return Ok(result);
        }

        [HttpGet("GetAllRelatedDeposit")]
        public async Task<IActionResult> GetAllRelated()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized();
            }

            var isGroupRelated = User.FindFirst("SwitchToGroup")?.Value;
            Guid? groupId = null;

            if (bool.TryParse(isGroupRelated, out var isGroupRelatedBool) && isGroupRelatedBool)
            {
                var groupIdClaim = User.FindFirst("GroupId")?.Value;
                if (!string.IsNullOrEmpty(groupIdClaim) && Guid.TryParse(groupIdClaim, out var groupGuid))
                {
                    groupId = groupGuid;
                }
            }

            var result = await _mediator.Send(new GetAllRelatedDepositsQuery(parsedUserId, groupId));
            return Ok(result);
        }

        [HttpGet("GetDeposit/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDepositByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("AddDeposit")]
        public async Task<IActionResult> AddDeposit([FromBody] DepositEntity deposit)
        {
            if (deposit.Amount <= 0)
                return BadRequest("Amount must be greater than 0.");

            if (!string.IsNullOrEmpty(deposit.Description) && deposit.Description.Length > 500)
                return BadRequest("Description max length is 500.");
            
            if (!string.IsNullOrEmpty(deposit.Tittle) && deposit.Tittle.Length > 500)
                return BadRequest("Tittle max length is 500.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized();
            }

            var isGroupRelated = User.FindFirst("SwitchToGroup")?.Value;
            if (bool.TryParse(isGroupRelated, out var isGroupRelatedBool) && isGroupRelatedBool)
            {
                var groupId = User.FindFirst("GroupId")?.Value;

                if (!string.IsNullOrEmpty(groupId) && Guid.TryParse(groupId, out var groupGuid))
                {
                    deposit.IsGroupRelated = true;
                    deposit.GroupId = groupGuid;
                }
            }

            deposit.AddedByUserId = parsedUserId;
            var result = await _mediator.Send(new CreateDepositCommand(deposit));
            return Ok(result);
        }

        [HttpPost("UpdateDeposit/{id}")]
        public async Task<IActionResult> UpdateDeposit(int id, [FromBody] DepositEntity deposit)
        {
            if (id != deposit.DepositId)
                return BadRequest("DepositId mismatch.");
            if (deposit.Amount <= 0)
                return BadRequest("Amount must be greater than 0.");
            if (!string.IsNullOrEmpty(deposit.Description) && deposit.Description.Length > 500)
                return BadRequest("Description max length is 500.");
            if (!string.IsNullOrEmpty(deposit.Tittle) && deposit.Tittle.Length > 500)
                return BadRequest("Tittle max length is 500.");

            var result = await _mediator.Send(new UpdateDepositCommand(id, deposit));
            return Ok(result);
        }

        [HttpDelete("DeleteDeposit/{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            var result = await _mediator.Send(new DeleteDepositCommand(id));
            return Ok(result);
        }
    }
}