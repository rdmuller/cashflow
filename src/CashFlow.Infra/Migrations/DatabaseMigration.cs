using CashFlow.Infra.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infra.Migrations;
public static class DatabaseMigration
{
    public static async Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbcontext = serviceProvider.GetRequiredService<CashFlowDbContext>();

        await dbcontext.Database.MigrateAsync();
    }
}
