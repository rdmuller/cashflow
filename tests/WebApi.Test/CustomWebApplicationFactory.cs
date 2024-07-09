using CashFlow.Domain.Security.Cryptografy;
using CashFlow.Infra.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private CashFlow.Domain.Entities.User _user;
    private string _password;

    public string GetEmail() => _user.Email;
    public string GetName() => _user.Name;
    public string GetPassword() => _password;

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

                StartDatabase(dbContext, passwordEncryptor);
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncryptor.Encrypt(_password);

        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
}
