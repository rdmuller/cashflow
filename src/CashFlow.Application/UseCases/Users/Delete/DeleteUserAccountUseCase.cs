
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete;
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserWriteOnlyRepository _repository;


    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser,
        IUserWriteOnlyRepository repository,
        IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        await _repository.Delete(loggedUser);

        await _unitOfWork.Commit();
    }
}
