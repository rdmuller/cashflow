using CashFlow.Communication.Requests;
using FluentAssertions;
using System.Text.Json;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : CashFlowClassFixture
{
    private const string METHOD = "api/login";
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;
    
    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.UserTeamMember.GetEmail();
        _name = webApplicationFactory.UserTeamMember.GetName();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(METHOD, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").ToString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").ToString().Should().NotBeNullOrWhiteSpace();
    }
}
