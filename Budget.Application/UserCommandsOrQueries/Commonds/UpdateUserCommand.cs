using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models.DTOs;

namespace WebApiBudget.Application.UserCommandsOrQueries.Commonds
{
    public record UpdateUserCommand(Guid UserId, UserDto User) : IRequest<UsersEntity>;
    
    public class UpdateUserCommandHandler(IUsersRepository usersRepository) : IRequestHandler<UpdateUserCommand, UsersEntity>
    {
        public async Task<UsersEntity> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await usersRepository.UpdateUsersAsync(request.UserId, request.User);
        }
    }
}
