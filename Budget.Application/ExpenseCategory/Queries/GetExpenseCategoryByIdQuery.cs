using MediatR;
using WebApiBudget.DomainOrCore.Entities;

namespace Budget.Application.ExpenseCategory.Queries
{
    public class GetExpenseCategoryByIdQuery : IRequest<ExpenseCategoryEntity?>
    {
        public int ExpenseCategoryID { get; set; }
    }
}
