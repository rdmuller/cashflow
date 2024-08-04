using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        var faker = new Faker();

        var request = new RequestExpenseJson
        {
            Title = faker.Commerce.Product(),
            Date = faker.Date.Past(),
            Amount = faker.Random.Decimal(min: 1, max: 1000),
            Description = faker.Commerce.ProductDescription(),
            PaymentType = faker.PickRandom<CashFlow.Communication.Enums.PaymentType>()
        };

        return request;

        /*return new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.Product())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductName());*/
    }
}
