namespace CashFlow.Domain.Security.Cryptografy;
public interface IPasswordEncryptor
{
    string Encrypt(string password);
    bool Verify(string password, string passwordHash);
}
