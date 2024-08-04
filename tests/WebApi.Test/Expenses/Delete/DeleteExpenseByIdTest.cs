using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses.Delete;
public class DeleteExpenseByIdTest : CashFlowClassFixture
{
    private const string METHOD = "api/expenses";
    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expenseId = webApplicationFactory.ExpenseTeamMember.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete($"{METHOD}/{_expenseId}", _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet($"{METHOD}/{_expenseId}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorExpenseNotFound(string cultureInfo)
    {
        var result = await DoDelete($"{METHOD}/123456", token: _token, culture: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(err => err.GetString()!.Equals(expectedMessage));
    }
}
