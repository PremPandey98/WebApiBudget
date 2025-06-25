using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.ExpenseRecords.Commands
{
    public record UpdateExpenseRecordCommand(int ExpenseID ,ExpenseRecordsEntity ExpenseRecord) : IRequest<ExpenseRecordsEntity>;

    public class UpdateExpenseRecordCommandHandler(IExpenseRecordsRepository expenseRecordsRepository) : IRequestHandler<UpdateExpenseRecordCommand,ExpenseRecordsEntity>
    {
        public async Task<ExpenseRecordsEntity> Handle(UpdateExpenseRecordCommand request, CancellationToken cancellationToken)
        {
            return await expenseRecordsRepository.UpdateExpenseRecordAsync(request.ExpenseID, request.ExpenseRecord);
        }
    }
}
