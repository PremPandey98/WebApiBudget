using MediatR;
using WebApiBudget.DomainOrCore.Entities;
using System.Collections.Generic;

namespace Budget.Application.ExpenseCategory.Queries
{
    public class GetAllExpenseCategoriesQuery : IRequest<IEnumerable<ExpenseCategoryEntity>>
    {
    }
}
