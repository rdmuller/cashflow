using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete;
public class DeleteUserAccountTest : CashFlowClassFixture
{
    private const string METHOD = "api/user";
    private readonly string _token;

    public DeleteUserAccountTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
