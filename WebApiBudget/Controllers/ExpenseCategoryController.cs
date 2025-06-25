using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Budget.Application.ExpenseCategory.Commands;
using Budget.Application.ExpenseCategory.Queries;
using WebApiBudget.DomainOrCore.Entities;

namespace WebApiBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExpenseCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryEntity>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllExpenseCategoriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseCategoryEntity>> GetById(int id)
        {
            var result = await _mediator.Send(new GetExpenseCategoryByIdQuery { ExpenseCategoryID = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ExpenseCategoryEntity>> Create(CreateExpenseCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.ExpenseCategoryID }, result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ExpenseCategoryEntity>> Update(UpdateExpenseCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteExpenseCategoryCommand { ExpenseCategoryID = id });
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
