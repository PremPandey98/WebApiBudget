using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Application.ExpenseCategory.Queries
{
    public class GetExpenseCategoryByIdQueryHandler : IRequestHandler<GetExpenseCategoryByIdQuery, ExpenseCategoryEntity?>
    {
        private readonly IExpenseCategoryRepository _repository;
        public GetExpenseCategoryByIdQueryHandler(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<ExpenseCategoryEntity?> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.ExpenseCategoryID);
        }
    }
}
