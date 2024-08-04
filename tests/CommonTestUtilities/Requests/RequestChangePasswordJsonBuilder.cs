using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(req => req.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(req => req.Password, faker => faker.Internet.Password());
    }
}
