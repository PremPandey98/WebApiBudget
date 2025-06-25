using MediatR;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.ExpenseRecords.Commands
{
    public class DeleteExpenseRecordCommand(int ExpenseId) : IRequest<bool>
    {
        public int ExpenseId { get; } = ExpenseId;
    }
    public class DeleteExpenseRecordCommandHandler(IExpenseRecordsRepository expenseRecordsRepository) : IRequestHandler<DeleteExpenseRecordCommand, bool>
    {
        public async Task<bool>Handle(DeleteExpenseRecordCommand request , CancellationToken cancellationToken)
        {
            if (request.ExpenseId == null)
            {
                throw new ArgumentException("Group ID cannot be empty", nameof(request.ExpenseId));
            }
            return await expenseRecordsRepository.DeleteExpenseRecordByIdAsync(request.ExpenseId);
        }
    }
}
