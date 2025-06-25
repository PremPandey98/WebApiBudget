using MediatR;
using WebApiBudget.DomainOrCore.Entities;

namespace Budget.Application.ExpenseCategory.Commands
{
    public class UpdateExpenseCategoryCommand : IRequest<ExpenseCategoryEntity>
    {
        public int ExpenseCategoryID { get; set; }
        public string ExpenseCategoryName { get; set; } = string.Empty;
    }
}
