using CashFlow.Application.UseCases.Expenses;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Expenses;
public class ExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Title_Empty()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.Now.AddDays(1);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Error_Amount_Invalid(decimal amount)
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amount;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }

    [Fact]
    public void Error_Tag_Invalid()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Tags.Add((CashFlow.Communication.Enums.Tag)15);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED));
    }
}