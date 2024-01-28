using UserServiceApp.Contracts.Common;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Common.Interfaces;
public interface IUserService
{
    Task DeleteUeserAsync(Guid userId, CancellationToken cancellationToken);

    Task<List<User>> GetAllUserDatasAsync(CancellationToken cancellationToken);

    Task<AuthenticationResult> LoginUserAsync(string email, string password, CancellationToken cancellationToken);
}