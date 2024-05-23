using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesReadOnlyRepository
{
    Task<Expense?> GetById(long id);

    Task<List<Expense>> GetAll();

    Task<List<Expense>> FilterByMonth(DateOnly date);
}
