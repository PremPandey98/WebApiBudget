using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiBudget.Application.Authentication.Commands;
using WebApiBudget.Application.Authentication.Models;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var response = await _mediator.Send(new LoginCommand(request.UserName, request.Password));

            if (response == null)
                return Unauthorized("Invalid credentials");

            return Ok(response);
        }
    }
}
