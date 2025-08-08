using MediatR;

namespace Budget.Application.Authentication.Commands
{
    public record SendPasswordResetCommand(string Email) : IRequest<bool>;
    
    public record ResetPasswordCommand(string Email, string OtpCode, string NewPassword) : IRequest<bool>;
}