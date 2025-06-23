using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.GroupCommandsOrQueries.Queries
{
    public class GetGroupByIdQuery(Guid GroupId) : IRequest<GroupEntity?> 
    {
        public Guid GroupId { get; } = GroupId;
    }
    public class GetGroupByIdQueryHandler(IGroupRepository groupRepository) : IRequestHandler<GetGroupByIdQuery, GroupEntity?>
    {
        public async Task<GroupEntity?> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.GroupId == Guid.Empty)
            {
                throw new ArgumentException("Group ID cannot be empty", nameof(request.GroupId));
            }
            return await groupRepository.GetGroupByIdOrGroupCodeAsync(request.GroupId ,null);
        }
    }
}
