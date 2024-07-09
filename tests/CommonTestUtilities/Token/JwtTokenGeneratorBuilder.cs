using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token;
public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(accesTokenGenerator => accesTokenGenerator.Generate(It.IsAny<User>())).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlJSUiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6ImI2NzlhNzQxLTc2MTQtNDExMi1iYzI3LWFhNDY0ZGEzMjAzYyIsIm5iZiI6MTcxOTc1ODU1NywiZXhwIjoxNzE5ODE4NTU3LCJpYXQiOjE3MTk3NTg1NTd9.rehDZKBfMcXaPWYRw8HKtSc3Qo2xKVTkMp-4q8PBwzQ");

        return mock.Object;
    }
}
