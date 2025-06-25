using MediatR;

namespace Budget.Application.ExpenseCategory.Commands
{
    public class DeleteExpenseCategoryCommand : IRequest<bool>
    {
        public int ExpenseCategoryID { get; set; }
    }
}
