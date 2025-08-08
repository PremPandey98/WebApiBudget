using Budget.Application.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBudget.Application.Authentication.Commands;
using WebApiBudget.Application.Authentication.Models;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IEmailService _emailService;

        public AuthController(ISender mediator, IEmailService emailService)
        {
            _mediator = mediator;
            _emailService = emailService;
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

        [HttpPost("switchTogroup/{groupId}")]
        [Authorize]
        public async Task<IActionResult> SwitchGroup(Guid groupId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var response = await _mediator.Send(new SwitchToGroupCommand(parsedUserId, groupId));
            if (response == null)
                return Unauthorized("Invalid Group ID or Someting went wrong");

            return Ok(response);
        }

        [HttpPost("test-email")]
        [AllowAnonymous]
        public async Task<IActionResult> TestEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            try
            {
                var result = await _emailService.SendEmailAsync(request.Email, "Test Email - Budget App", 
                    "<h2>Test Email</h2><p>This is a test email from Budget App. If you receive this, email configuration is working!</p>");
                
                if (result)
                    return Ok(new { Success = true, Message = "Test email sent successfully" });
                else
                    return BadRequest(new { Success = false, Message = "Failed to send test email. Check logs for details." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("send-email-verification")]
        [AllowAnonymous]
        public async Task<IActionResult> SendEmailVerification([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            var result = await _mediator.Send(new SendEmailVerificationCommand(request.Email));
            
            if (result)
                return Ok(new { Success = true, Message = "Verification email sent successfully" });
            else
                return BadRequest(new { Success = false, Message = "Failed to send verification email" });
        }

        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.OtpCode))
                return BadRequest("Email and OTP code are required");

            var result = await _mediator.Send(new VerifyEmailCommand(request.Email, request.OtpCode));
            
            if (result)
                return Ok(new { Success = true, Message = "Email verified successfully" });
            else
                return BadRequest(new { Success = false, Message = "Invalid or expired OTP code" });
        }

        [HttpPost("send-password-reset")]
        [AllowAnonymous]
        public async Task<IActionResult> SendPasswordReset([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            var result = await _mediator.Send(new SendPasswordResetCommand(request.Email));
            
            if (result)
                return Ok(new { Success = true, Message = "Password reset email sent successfully" });
            else
                return BadRequest(new { Success = false, Message = "Failed to send password reset email" });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.OtpCode) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("Email, OTP code, and new password are required");

            var result = await _mediator.Send(new ResetPasswordCommand(request.Email, request.OtpCode, request.NewPassword));
            
            if (result)
                return Ok(new { Success = true, Message = "Password reset successfully" });
            else
                return BadRequest(new { Success = false, Message = "Invalid or expired OTP code" });
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; } = null!;
    }

    public class VerifyEmailRequest
    {
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
