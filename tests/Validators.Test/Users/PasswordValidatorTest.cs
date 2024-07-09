using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test.Users;
public class PasswordValidatorTest
{
    [Theory]
    [InlineData("  ")]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("Aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaa1")]
    [InlineData("Aaaaaaa!")]
    [InlineData(null)]
    public void ErrorPasswordInvalid(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();
        
        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        result.Should().BeFalse();
    }

    [Fact]
    public void Success()
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), "!1Aaaaaa");

        result.Should().BeTrue();
    }
}
