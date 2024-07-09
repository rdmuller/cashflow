using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess;
public class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }

    public DbSet<User> Users { get; set; }

   /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=cashflow;Uid=root;Pwd=docker";

        var serverVersion = new MySqlServerVersion(new Version(8, 1, 0));

        optionsBuilder.UseMySql(connectionString, serverVersion);
    }*/
}
