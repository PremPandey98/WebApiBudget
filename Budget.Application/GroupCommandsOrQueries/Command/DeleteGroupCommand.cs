using MediatR;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.GroupCommandsOrQueries.Command
{
    public class DeleteGroupCommand(Guid GroupId) : IRequest<bool>
    {
        public Guid GroupId { get; } = GroupId;
    }
    public class DeleteGroupCommandHandler(IGroupRepository groupRepository) : IRequestHandler<DeleteGroupCommand, bool>
    {
        public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            if (request.GroupId == Guid.Empty)
            {
                throw new ArgumentException("Group ID cannot be empty", nameof(request.GroupId));
            }
            return await groupRepository.DeleteGroupByIdAsync(request.GroupId);
        }
    }
}
