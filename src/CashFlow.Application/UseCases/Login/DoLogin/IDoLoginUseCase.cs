using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Domain.Login;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
