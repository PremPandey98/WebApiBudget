using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.Deposits.Queries
{
    public class GetAllDepositsQuery : IRequest<IEnumerable<DepositEntity>>;

    public class GetAllDepositsQueryHandler(IDepositRepository depositRepository) : IRequestHandler<GetAllDepositsQuery, IEnumerable<DepositEntity>>
    {
        public async Task<IEnumerable<DepositEntity>> Handle(GetAllDepositsQuery request, CancellationToken cancellationToken)
        {
            return await depositRepository.GetAllDepositAsync();
        }
    }
}