using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.GroupCommandsOrQueries.Queries
{
    public class GetAllGroupsQuery() : IRequest<IEnumerable<GroupEntity>>;

    public class GetAllGroupQurieHandler(IGroupRepository groupRepository) : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupEntity>>
    {
        public async Task<IEnumerable<GroupEntity>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            return await groupRepository.GetAllGroupsAsync();
        }
    }
}
