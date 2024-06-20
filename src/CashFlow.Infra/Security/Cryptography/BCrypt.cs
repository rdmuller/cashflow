using CashFlow.Domain.Security.Cryptografy;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infra.Security.Cryptography;
public class BCrypt : IPasswordEncryptor
{
    public string Encrypt(string password)
    {
        var passwordHash = BC.HashPassword(password);
        return passwordHash;
    }

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}
