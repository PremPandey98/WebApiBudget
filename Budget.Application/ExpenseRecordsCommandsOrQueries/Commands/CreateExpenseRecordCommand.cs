using System;
using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace Budget.Application.ExpenseRecords.Commands
{
    public record CreateExpenseRecordCommand(ExpenseRecordsEntity ExpenseRecord) : IRequest<ExpenseRecordsEntity>;

    public class CreateExpenseRecordCommandHandler(IExpenseRecordsRepository expenseRecordsRepository, IExpenseCategoryRepository expenseCategoryRepository, IUsersRepository usersRepository,IGroupRepository groupRepository) : IRequestHandler<CreateExpenseRecordCommand, ExpenseRecordsEntity>
    {
        public async Task<ExpenseRecordsEntity> Handle(CreateExpenseRecordCommand request, CancellationToken cancellationToken)
        {
            if (request.ExpenseRecord == null)
            {
                throw new ArgumentNullException(nameof(request.ExpenseRecord), "Group cannot be null");
            }
            if (request.ExpenseRecord != null && request.ExpenseRecord.ExpenseCategoryID != 0)
            {
                var id = request.ExpenseRecord.ExpenseCategoryID;
                var expenseCategory = await expenseCategoryRepository.GetByIdAsync(id);
                request.ExpenseRecord.ExpenseCategory = expenseCategory;
            }
            if(request.ExpenseRecord != null && request.ExpenseRecord.AddedByUserId != null)
            {
                var Id = request.ExpenseRecord.AddedByUserId;
                var User = await usersRepository.GetUserByIdOrUserNameAsync(Id, null);
                request.ExpenseRecord.AddedByUser = User ?? null;
            }
            if (request.ExpenseRecord != null && request.ExpenseRecord.IsGroupRelated == true && request.ExpenseRecord.GroupId != null)
            {
                var Id = request.ExpenseRecord.GroupId;
                var group = await groupRepository.GetGroupByIdOrGroupCodeAsync((Guid)Id,null);
                request.ExpenseRecord.Group = (GroupEntity?)(group ?? null);
            }

            return await expenseRecordsRepository.AddExpenseRecordAsync(request.ExpenseRecord);
        }
    }

}
