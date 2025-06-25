using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Application.ExpenseCategory.Commands
{
    public class CreateExpenseCategoryCommandHandler : IRequestHandler<CreateExpenseCategoryCommand, ExpenseCategoryEntity>
    {
        private readonly IExpenseCategoryRepository _repository;
        public CreateExpenseCategoryCommandHandler(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<ExpenseCategoryEntity> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new ExpenseCategoryEntity { ExpenseCategoryName = request.ExpenseCategoryName };
            return await _repository.AddAsync(entity);
        }
    }
}
