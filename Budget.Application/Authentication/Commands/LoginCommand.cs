using MediatR;
using WebApiBudget.Application.Authentication.Interfaces;
using WebApiBudget.Application.Authentication.Models;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.Authentication.Commands
{
    public record LoginCommand(string UserName, string Password) : IRequest<AuthResponse?>;

    public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, AuthResponse?>
    {
        public async Task<AuthResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await authService.ValidateUserAsync(request.UserName, request.Password);
            
            if (user == null)
                return null;
                
            var token = await authService.GenerateTokenAsync(user);
            
            return new AuthResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }
    }
}
