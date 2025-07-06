using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.Deposits.Queries
{
    public record GetDepositByIdQuery(int DepositId) : IRequest<DepositEntity>;
    
    public class GetDepositByIdQueryHandler : IRequestHandler<GetDepositByIdQuery, DepositEntity>
    {
        private readonly IDepositRepository _repository;
        public GetDepositByIdQueryHandler(IDepositRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<DepositEntity> Handle(GetDepositByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetDepositByIdAsync(request.DepositId);
        }
    }
}