using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.Deposits.Queries
{
    public class GetAllRelatedDepositsQuery : IRequest<IEnumerable<DepositEntity>>
    {
        public Guid UserId { get; }
        public Guid? GroupId { get; }

        public GetAllRelatedDepositsQuery(Guid userId, Guid? groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    public class GetAllRelatedDepositsQueryHandler : IRequestHandler<GetAllRelatedDepositsQuery, IEnumerable<DepositEntity>>
    {
        private readonly IDepositRepository _depositRepository;

        public GetAllRelatedDepositsQueryHandler(IDepositRepository depositRepository)
        {
            _depositRepository = depositRepository;
        }

        public async Task<IEnumerable<DepositEntity>> Handle(GetAllRelatedDepositsQuery request, CancellationToken cancellationToken)
        {
            return await _depositRepository.GetAllRelatedDepositsAsync(request.UserId, request.GroupId ?? Guid.Empty);
        }
    }
}