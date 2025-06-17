using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.UserCommandsOrQueries.Commonds
{
    public record UpdateUserCommand(Guid UserId, UsersEntity User) : IRequest<UsersEntity>;
    
    public class UpdateUserCommandHandler(IUsersRepository usersRepository) : IRequestHandler<UpdateUserCommand, UsersEntity>
    {
        public async Task<UsersEntity> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await usersRepository.UpdateUsersAsync(request.UserId, request.User);
        }
    }
}
