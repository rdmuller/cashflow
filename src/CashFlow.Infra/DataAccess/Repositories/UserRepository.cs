using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptografy;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
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

    public async Task Delete(User user)
    {
        var userToRemove = await _dbContext.Users.FindAsync(user.Id);
        _dbContext.Users.Remove(userToRemove!);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email));
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.FirstAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email));

        return user;
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}
