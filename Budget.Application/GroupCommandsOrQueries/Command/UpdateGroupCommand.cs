using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.GroupCommandsOrQueries.Command
{
    public record UpdateGroupCommand(Guid GroupId, GroupEntity Group) : IRequest<GroupEntity>;
    public class UpdateGroupCommandHandler(IGroupRepository groupRepository) : IRequestHandler<UpdateGroupCommand, GroupEntity>
    {
        public async Task<GroupEntity> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            // Validate the group entity here if needed
            return await groupRepository.UpdateGroupAsync(request.GroupId, request.Group);
        }
    }

}

