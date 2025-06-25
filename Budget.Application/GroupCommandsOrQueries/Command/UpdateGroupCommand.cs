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
            Validators.AddGroupCommandValidator validator = new(groupRepository);

            if (!await validator.ValidateAsync(request.Group))
            {
                throw new ArgumentException("Group validation failed");
            }
            return await groupRepository.UpdateGroupAsync(request.GroupId, request.Group);
        }
    }

}

