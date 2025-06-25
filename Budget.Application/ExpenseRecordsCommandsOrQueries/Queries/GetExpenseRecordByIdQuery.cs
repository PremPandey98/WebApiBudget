using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.ExpenseRecords.Queries
{
    public record GetExpenseRecordByIdQuery(int ExpenseId) : IRequest<ExpenseRecordsEntity>;
    public class GetExpenseRecordByIdQueryHandler : IRequestHandler<GetExpenseRecordByIdQuery, ExpenseRecordsEntity>
    {
        private readonly IExpenseRecordsRepository _repository;
        public GetExpenseRecordByIdQueryHandler(IExpenseRecordsRepository repository)
        {
            _repository = repository;
        }
        public async Task<ExpenseRecordsEntity> Handle(GetExpenseRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetExpenseRecordByIdAsync(request.ExpenseId);
        }
    }
}
