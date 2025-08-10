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

    public class GetGroupUserCountQuery(Guid GroupId) : IRequest<int>
    {
        public Guid GroupId { get; } = GroupId;
    }

    public class GetGroupUserCountQueryHandler(IGroupRepository groupRepository) : IRequestHandler<GetGroupUserCountQuery, int>
    {
        public async Task<int> Handle(GetGroupUserCountQuery request, CancellationToken cancellationToken)
        {
            if (request.GroupId == Guid.Empty)
            {
                throw new ArgumentException("Group ID cannot be empty", nameof(request.GroupId));
            }
            
            var group = await groupRepository.GetGroupByIdOrGroupCodeAsync(request.GroupId, null);
            return group?.Users?.Count ?? 0;
        }
    }
}
