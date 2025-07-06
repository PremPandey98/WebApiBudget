using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.Deposits.Commands
{
    public record UpdateDepositCommand(int DepositID, DepositEntity Deposit) : IRequest<DepositEntity>;

    public class UpdateDepositCommandHandler(IDepositRepository depositRepository) : IRequestHandler<UpdateDepositCommand, DepositEntity>
    {
        public async Task<DepositEntity> Handle(UpdateDepositCommand request, CancellationToken cancellationToken)
        {
            return await depositRepository.UpdateDepositAsync(request.DepositID, request.Deposit);
        }
    }
}