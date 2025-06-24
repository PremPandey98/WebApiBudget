using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Budget.Application.UserCommandsOrQueries.Commonds
{
    public record RemoveUserGroupCommand(Guid UserId, Guid GroupId) : IRequest<UsersEntity>;
    public class RemoveUserGroupCommandHandler(IUsersRepository usersRepository) : IRequestHandler<RemoveUserGroupCommand, UsersEntity>
    {
        public async Task<UsersEntity> Handle(RemoveUserGroupCommand request, CancellationToken cancellationToken)
        {
            return await usersRepository.RemoveUserGroupAsync(request.UserId, request.GroupId);
        }
    }
}
