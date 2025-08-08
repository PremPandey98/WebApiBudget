using MediatR;

namespace WebApiBudget.Application.Authentication.Commands
{
    public record SendEmailVerificationCommand(string Email) : IRequest<bool>;
    
    public record VerifyEmailCommand(string Email, string OtpCode) : IRequest<bool>;
}