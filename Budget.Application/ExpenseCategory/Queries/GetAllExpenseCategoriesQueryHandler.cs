using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Application.ExpenseCategory.Queries
{
    public class GetAllExpenseCategoriesQueryHandler : IRequestHandler<GetAllExpenseCategoriesQuery, IEnumerable<ExpenseCategoryEntity>>
    {
        private readonly IExpenseCategoryRepository _repository;
        public GetAllExpenseCategoriesQueryHandler(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ExpenseCategoryEntity>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
