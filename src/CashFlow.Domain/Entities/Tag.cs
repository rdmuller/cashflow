namespace CashFlow.Domain.Entities;
public class Tag
{
    public int Id { get; set; }
    public Enums.Tag Value { get; set; }

    public long ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}
