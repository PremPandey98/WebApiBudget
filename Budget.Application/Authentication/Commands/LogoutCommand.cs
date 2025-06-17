using MediatR;
using WebApiBudget.Application.Authentication.Models;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.Authentication.Commands
{
    public record LogoutCommand(Guid UserId, string Token) : IRequest<bool>;

    public class LogoutCommandHandler(ITokenBlacklistRepository tokenBlacklistRepository) : IRequestHandler<LogoutCommand, bool>
    {
        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await tokenBlacklistRepository.AddToBlacklistAsync(request.Token, request.UserId);
        }
    }
}
