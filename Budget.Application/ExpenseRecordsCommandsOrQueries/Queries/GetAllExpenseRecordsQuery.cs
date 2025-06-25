using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.ExpenseRecordsCommandsOrQueries.Queries
{
    public class GetAllExpenseRecordsQuery : IRequest<IEnumerable<ExpenseRecordsEntity>>;

    public class GetAllExpenseRecordsQueryHandler(IExpenseRecordsRepository expenseRecordsRepository) : IRequestHandler<GetAllExpenseRecordsQuery, IEnumerable<ExpenseRecordsEntity>>
    {
        public async Task<IEnumerable<ExpenseRecordsEntity>> Handle(GetAllExpenseRecordsQuery request, CancellationToken cancellationToken)
        {
            return await expenseRecordsRepository.GetAllExpenseRecordAsync();
        }
    }
}
