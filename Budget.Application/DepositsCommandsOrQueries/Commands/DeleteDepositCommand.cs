using MediatR;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.Deposits.Commands
{
    public class DeleteDepositCommand(int DepositId) : IRequest<bool>
    {
        public int DepositId { get; } = DepositId;
    }
    
    public class DeleteDepositCommandHandler(IDepositRepository depositRepository) : IRequestHandler<DeleteDepositCommand, bool>
    {
        public async Task<bool> Handle(DeleteDepositCommand request, CancellationToken cancellationToken)
        {
            if (request.DepositId <= 0)
            {
                throw new ArgumentException("Deposit ID cannot be empty", nameof(request.DepositId));
            }
            return await depositRepository.DeleteDepositByIdAsync(request.DepositId);
        }
    }
}