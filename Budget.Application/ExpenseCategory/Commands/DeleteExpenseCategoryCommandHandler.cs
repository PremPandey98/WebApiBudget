using MediatR;
using WebApiBudget.DomainOrCore.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Application.ExpenseCategory.Commands
{
    public class DeleteExpenseCategoryCommandHandler : IRequestHandler<DeleteExpenseCategoryCommand, bool>
    {
        private readonly IExpenseCategoryRepository _repository;
        public DeleteExpenseCategoryCommandHandler(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.ExpenseCategoryID);
        }
    }
}
