using CashFlow.Domain.Security.Cryptografy;
using Moq;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncryptorBuilder
{
    private readonly Mock<IPasswordEncryptor> _mock;

    public PasswordEncryptorBuilder()
    {
        _mock = new Mock<IPasswordEncryptor>();
        _mock.Setup(passwordEncryptor => passwordEncryptor.Encrypt(It.IsAny<string>())).Returns("!%dlfjkd545");
    }

    public PasswordEncryptorBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
        {
            _mock.Setup(passwordEncryptor => passwordEncryptor.Verify(password, It.IsAny<string>())).Returns(true);
        }

        return this;
    }

    public IPasswordEncryptor Build() => _mock.Object;
}
