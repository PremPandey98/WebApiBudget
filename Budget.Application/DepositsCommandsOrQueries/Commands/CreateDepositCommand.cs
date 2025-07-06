using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.Deposits.Commands
{
    public record CreateDepositCommand(DepositEntity Deposit) : IRequest<DepositEntity>;

    public class CreateDepositCommandHandler(IDepositRepository depositRepository, IUsersRepository usersRepository, IGroupRepository groupRepository) : IRequestHandler<CreateDepositCommand, DepositEntity>
    {
        public async Task<DepositEntity> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            if (request.Deposit == null)
            {
                throw new ArgumentNullException(nameof(request.Deposit), "Deposit cannot be null");
            }
            
            if(request.Deposit != null && request.Deposit.AddedByUserId != null)
            {
                var Id = request.Deposit.AddedByUserId;
                var User = await usersRepository.GetUserByIdOrUserNameAsync(Id, null);
                request.Deposit.AddedByUser = User ?? null;
            }
            
            if (request.Deposit != null && request.Deposit.IsGroupRelated == true && request.Deposit.GroupId != null)
            {
                var Id = request.Deposit.GroupId;
                var group = await groupRepository.GetGroupByIdOrGroupCodeAsync((Guid)Id,null);
                request.Deposit.Group = (GroupEntity?)(group ?? null);
            }

            return await depositRepository.AddDepositAsync(request.Deposit);
        }
    }
}