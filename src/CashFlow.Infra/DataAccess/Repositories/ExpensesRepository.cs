using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess.Repositories;
internal class ExpensesRepository : IExpensesWriteOnlyRepository, IExpensesReadOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        //var dbContext = new CashFlowDbContext();
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        // AsNoTracking gera uma otimização, pois não controla os registros no cache; 
        // porém usar apenas quando não alterar dados
        return await _dbContext.Expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpense()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpense()
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    public async Task Delete(long id)
    {
        var expense = await _dbContext.Expenses.FirstAsync(e => e.Id == id);

        _dbContext.Expenses.Remove(expense);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: lastDay, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate && e.UserId == user.Id)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync();
    }

    private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
    {
        return _dbContext.Expenses
            .Include(expense => expense.Tags);
    }
}
