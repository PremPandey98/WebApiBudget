using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.GroupCommandsOrQueries.Command
{
    public record AddGroupCommand(GroupEntity User) : IRequest<GroupEntity>;
     
    public class AddGroupCommandHandler(IGroupRepository groupRepository) : IRequestHandler<AddGroupCommand, GroupEntity>
    {
        public async Task<GroupEntity> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new ArgumentNullException(nameof(request.User), "Group cannot be null");
            }
            // Validate the group entity here if needed
            return await groupRepository.AddGroupAsync(request.User);
        }
    }
}
