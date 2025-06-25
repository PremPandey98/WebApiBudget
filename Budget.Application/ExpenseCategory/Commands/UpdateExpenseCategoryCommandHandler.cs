using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Application.ExpenseCategory.Commands
{
    public class UpdateExpenseCategoryCommandHandler : IRequestHandler<UpdateExpenseCategoryCommand, ExpenseCategoryEntity>
    {
        private readonly IExpenseCategoryRepository _repository;
        public UpdateExpenseCategoryCommandHandler(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<ExpenseCategoryEntity> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new ExpenseCategoryEntity { ExpenseCategoryID = request.ExpenseCategoryID, ExpenseCategoryName = request.ExpenseCategoryName };
            return await _repository.UpdateAsync(entity);
        }
    }
}
