using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.Queries
{
    public class GetUserByIdQuery(Guid UserId) : IRequest<UsersEntity?>
    {
        public Guid UserId { get; } = UserId;
    }
    public class GetUserByIdQueryHandler(IUsersRepository usersRepository) : IRequestHandler<GetUserByIdQuery, UsersEntity?>
    {
        public async Task<UsersEntity?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty", nameof(request.UserId));
            }
            return await usersRepository.GetUserByIdAsync(request.UserId);
        }
    }

}
