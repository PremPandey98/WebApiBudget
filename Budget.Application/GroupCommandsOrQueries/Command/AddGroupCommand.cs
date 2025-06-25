using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.GroupCommandsOrQueries.Command
{
    public record AddGroupCommand(GroupEntity Group) : IRequest<GroupEntity>;
     
    public class AddGroupCommandHandler(IGroupRepository groupRepository) : IRequestHandler<AddGroupCommand, GroupEntity>
    {
        public async Task<GroupEntity> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            if (request.Group == null)
            {
                throw new ArgumentNullException(nameof(request.Group), "Group cannot be null");
            }
            Validators.AddGroupCommandValidator validator = new(groupRepository);
            
            if (!await validator.ValidateAsync(request.Group))
            {
                throw new ArgumentException("Group validation failed");
            }
            return await groupRepository.AddGroupAsync(request.Group);
        }
    }
}
