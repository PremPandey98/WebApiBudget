using MediatR;
using WebApiBudget.DomainOrCore.Entities;

namespace Budget.Application.ExpenseCategory.Commands
{
    public class CreateExpenseCategoryCommand : IRequest<ExpenseCategoryEntity>
    {
        public string ExpenseCategoryName { get; set; } = string.Empty;
    }
}
