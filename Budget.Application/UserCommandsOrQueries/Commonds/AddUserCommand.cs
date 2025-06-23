using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.UserCommandsOrQueries.Commonds
{
    public record AddUserCommand(UsersEntity User) : IRequest<UsersEntity>;

    public class AddUserCommondResponse(IUsersRepository usersRepository) : IRequestHandler<AddUserCommand, UsersEntity>
    {
        public async Task<UsersEntity> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new ArgumentNullException(nameof(request.User), "User cannot be null");
            }
            Validators.AddUserCommandValidator validator = new(usersRepository);
           
            if (!await validator.ValidateAsync(request.User))
            {
                throw new ArgumentException("User validation failed");
            }
            return await usersRepository.AddUsersAsync(request.User);
        }
    }

}
