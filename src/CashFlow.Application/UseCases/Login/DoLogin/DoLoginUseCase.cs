using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Login;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptografy;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public DoLoginUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IPasswordEncryptor passwordEncryptor)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);
        if (user is null)
        {
            throw new InvalidLoginException();
        }

        var passwordMatch = _passwordEncryptor.Verify(request.Password, user.Password);
        if (!passwordMatch)
        {
            throw new InvalidLoginException();
        };

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}
