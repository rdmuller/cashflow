using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptografy;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public UserRepository(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor)
    {  
        _dbContext = dbContext; 
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email));

        return user;
    }
}
