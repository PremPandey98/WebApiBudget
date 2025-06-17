using MediatR;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.UserCommandsOrQueries.Commonds
{
    public record DeleteUserCommand(Guid UserId) : IRequest<bool>;
    public class DeleteUserCommandHandler(IUsersRepository usersRepository) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty", nameof(request.UserId));
            }
            return await usersRepository.DeleteUsersByIdAsync(request.UserId);
        }
    }
}
