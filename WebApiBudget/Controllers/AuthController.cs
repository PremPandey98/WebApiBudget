using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                {
                    return Unauthorized();
                }
                
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                
                var result = await _mediator.Send(new LogoutCommand(parsedUserId, token));
                
                if (result)
                {
                    return Ok(new LogoutResponse { Success = true, Message = "Successfully logged out" });
                }
                else
                {
                    return BadRequest(new LogoutResponse { Success = false, Message = "Failed to logout" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LogoutResponse { Success = false, Message = $"Error during logout: {ex.Message}" });
            }
        }
    }
}
