using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.UserCommandsOrQueries.Queries
{
    public class GetAllUsersQuery() : IRequest<IEnumerable<UsersEntity>>;
    public class GetAllUsersQueryHandler(IUsersRepository usersRepository) : IRequestHandler<GetAllUsersQuery, IEnumerable<UsersEntity>>
    {
        public async Task<IEnumerable<UsersEntity>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await usersRepository.GetAllUsersAsync();
        }
    }

}
