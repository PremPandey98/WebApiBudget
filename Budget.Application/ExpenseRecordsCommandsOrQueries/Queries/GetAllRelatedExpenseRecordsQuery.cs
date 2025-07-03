using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.ExpenseRecordsCommandsOrQueries.Queries
{
    public class GetAllRelatedExpenseRecordsQuery : IRequest<IEnumerable<ExpenseRecordsEntity>>
    {
        public Guid UserId { get; }
        public Guid? GroupId { get; }

        public GetAllRelatedExpenseRecordsQuery(Guid userId, Guid? groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    public class GetAllRelatedExpenseRecordsQueryHandler : IRequestHandler<GetAllRelatedExpenseRecordsQuery, IEnumerable<ExpenseRecordsEntity>>
    {
        private readonly IExpenseRecordsRepository _expenseRecordsRepository;

        public GetAllRelatedExpenseRecordsQueryHandler(IExpenseRecordsRepository expenseRecordsRepository)
        {
            _expenseRecordsRepository = expenseRecordsRepository;
        }

        public async Task<IEnumerable<ExpenseRecordsEntity>> Handle(GetAllRelatedExpenseRecordsQuery request, CancellationToken cancellationToken)
        {
            return await _expenseRecordsRepository.GetAllRelatedExpenseRecordsAsync(request.UserId, request.GroupId?? Guid.Empty);
        }
    }
}






