using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models.DTOs;


namespace WebApiBudget.Application.UserCommandsOrQueries.Commonds
{
    public class UserGroupUpdateCommand : IRequest<UsersEntity>
    {
        public Guid UserId { get; }
        public GroupDto Group { get; }

        public UserGroupUpdateCommand(Guid userId, GroupDto group)
        {
            UserId = userId;
            Group = group;
        }
    }

    public class UserGroupUpdateCommandHandler : IRequestHandler<UserGroupUpdateCommand, UsersEntity>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IGroupRepository _groupRepository; 

        public UserGroupUpdateCommandHandler(IUsersRepository usersRepository, IGroupRepository groupRepository)
        {
            _usersRepository = usersRepository;
            _groupRepository = groupRepository;
        }

        public async Task<UsersEntity> Handle(UserGroupUpdateCommand request, CancellationToken cancellationToken)
        {
            Validators.UpdateUserGroupCommandValidator validator = new(_groupRepository);
            
            var groupRecord = await validator.ValidateAsync(request.Group);
            
            return await _usersRepository.UpdateUserGroupsAsync(request.UserId, groupRecord);
        }
    }
}

