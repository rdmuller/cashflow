using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;
public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string METHOD = "api/report";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;

    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _teamMemberToken = webApplicationFactory.UserTeamMember.GetToken();
        _adminToken = webApplicationFactory.UserAdmin.GetToken();
        _expenseDate = webApplicationFactory.ExpenseAdmin.GetDate();
    }

    [Fact]
    public async Task SuccessPdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:MM}/{_expenseDate:yyyy}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task SuccessExcel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:MM}/{_expenseDate:yyyy}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task ErrorExcelNotAllowed()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:MM}/{_expenseDate:yyyy}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ErrorPdfNotAllowed()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:MM}/{_expenseDate:yyyy}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
