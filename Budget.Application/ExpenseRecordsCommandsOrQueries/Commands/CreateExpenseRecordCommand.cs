using System;
using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.ExpenseRecords.Commands
{
    public record CreateExpenseRecordCommand(ExpenseRecordsEntity ExpenseRecord) : IRequest<ExpenseRecordsEntity>;

    public class CreateExpenseRecordCommandHandler(IExpenseRecordsRepository expenseRecordsRepository) : IRequestHandler<CreateExpenseRecordCommand, ExpenseRecordsEntity>
    {
        public async Task<ExpenseRecordsEntity>Handle(CreateExpenseRecordCommand request, CancellationToken cancellationToken)
        {
            if (request.ExpenseRecord == null)
            {
                throw new ArgumentNullException(nameof(request.ExpenseRecord), "Group cannot be null");
            }
  
            return await expenseRecordsRepository.AddExpenseRecordAsync(request.ExpenseRecord);
        }
    }

}
