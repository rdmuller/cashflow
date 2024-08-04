using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptografy;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infra.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager ExpenseAdmin { get; private set; } = default!; // default serve para dizer q não vai ser nulo
    public ExpenseIdentityManager ExpenseTeamMember { get; private set; } = default!; // default serve para dizer q não vai ser nulo
    public UserIdentityManager UserTeamMember {  get; private set; } = default!;
    public UserIdentityManager UserAdmin {  get; private set; } = default!;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //base.ConfigureWebHost(builder);
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scoped = services.BuildServiceProvider().CreateScope();
                var dbContext = scoped.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncryptor = scoped.ServiceProvider.GetRequiredService<IPasswordEncryptor>();
                var accessTokenGenerator = scoped.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncryptor, accessTokenGenerator);
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor, IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncryptor, accessTokenGenerator);
        var expense = AddExpenses(dbContext, userTeamMember, expenseId: 1);
        ExpenseTeamMember = new ExpenseIdentityManager(expense);

        var userAdmin = AddUserAdmin(dbContext, passwordEncryptor, accessTokenGenerator);
        expense = AddExpenses(dbContext, userAdmin, expenseId: 2);
        ExpenseAdmin = new ExpenseIdentityManager(expense);

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(
        CashFlowDbContext dbContext,
        IPasswordEncryptor passwordEncryptor,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = passwordEncryptor.Encrypt(password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        UserTeamMember = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(
        CashFlowDbContext dbContext,
        IPasswordEncryptor passwordEncryptor,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = passwordEncryptor.Encrypt(password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        UserAdmin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        dbContext.Expenses.Add(expense);

        return expense;
    }
}
