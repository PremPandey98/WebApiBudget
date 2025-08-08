using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Application.ExpenseRecords.Commands;
using Budget.Application.ExpenseRecords.Queries;
using MediatR;
using WebApiBudget.Application.ExpenseRecordsCommandsOrQueries.Queries;
using Microsoft.AspNetCore.Authorization;
using WebApiBudget.DomainOrCore.Entities;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WebApiBudget.Infrastucture.Services;

namespace WebApiBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UsageTrackingService _usageTrackingService;
        
        public ExpenseRecordsController(IMediator mediator, UsageTrackingService usageTrackingService)
        {
            _mediator = mediator;
            _usageTrackingService = usageTrackingService;
        }

        [HttpGet("GetAllExpenseRecord")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllExpenseRecordsQuery());
            return Ok(result);
        }

        [HttpGet("GetAllRelatedExpenseRecord")]
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

            var result = await _mediator.Send(new GetAllRelatedExpenseRecordsQuery(parsedUserId, groupId));
            return Ok(result);
        }

        [HttpGet("GetExpenseRecord/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetExpenseRecordByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("AddExpenseRecord")]
        public async Task<IActionResult> AddExpenseRecord([FromBody] ExpenseRecordsEntity expenseRecord)
        {
            
            if (expenseRecord.Amount <= 0)
                return BadRequest("Amount must be greater than 0.");

            if (!string.IsNullOrEmpty(expenseRecord.Description) && expenseRecord.Description.Length > 500)
                return BadRequest("Description max length is 500.");
            
            if (!string.IsNullOrEmpty(expenseRecord.Tittle) && expenseRecord.Tittle.Length > 500)
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
                    expenseRecord.IsGroupRelated = true;
                    expenseRecord.GroupId = groupGuid;
                }

            }

            expenseRecord.AddedByUserId = parsedUserId;
            var result = await _mediator.Send(new CreateExpenseRecordCommand(expenseRecord));
            
            // Send notification if it's a group expense
            if (expenseRecord.IsGroupRelated && expenseRecord.GroupId.HasValue)
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Someone";
                var title = "New Group Expense";
                var body = $"{userName} added a new expense: ${expenseRecord.Amount:F2} for {expenseRecord.Tittle ?? "Expense"}";
                
            }
            
            // Check individual user's high usage
            _ = Task.Run(async () =>
            {
                try
                {
                    await _usageTrackingService.CheckHighUsageAsync(parsedUserId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Usage tracking failed: {ex.Message}");
                }
            });
            
            return Ok(result);
        }

        [HttpPost("UpdateexpenseRecord/{id}")]
        public async Task<IActionResult> UpdateExpenseRecord(int id, [FromBody] ExpenseRecordsEntity expenseRecord)
        {
            if (id != expenseRecord.ExpenseId)
                return BadRequest("ExpenseId mismatch.");
            if (expenseRecord.Amount <= 0)
                return BadRequest("Amount must be greater than 0.");
            if (!string.IsNullOrEmpty(expenseRecord.Description) && expenseRecord.Description.Length > 500)
                return BadRequest("Description max length is 500.");

            var result = await _mediator.Send(new UpdateExpenseRecordCommand(id, expenseRecord));
            return Ok(result);
        }

        [HttpDelete("DeleteExpenseRecord/{id}")]
        public async Task<IActionResult> DeleteExpenseRecord(int id)
        {
            var result = await _mediator.Send(new DeleteExpenseRecordCommand(id));
            return Ok(result);
        }
    }
}
