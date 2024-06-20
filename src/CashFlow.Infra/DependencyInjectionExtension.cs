using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptografy;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infra.DataAccess;
using CashFlow.Infra.DataAccess.Repositories;
using CashFlow.Infra.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infra;
public static class DependencyInjectionExtension
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
        AddToken(services, configuration);

        services.AddScoped<IPasswordEncryptor, Security.Cryptography.BCrypt>();
    }

    public static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");  // para ler do appsettings "Server=localhost;Database=cashflow;Uid=root;Pwd=docker";
        var serverVersion = new MySqlServerVersion(new Version(8, 1, 0));

        services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    public static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }
}
