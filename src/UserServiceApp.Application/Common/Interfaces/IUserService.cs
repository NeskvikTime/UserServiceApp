using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Common.Interfaces;
public interface IUserService
{
    public Task DeleteUeserAsync(Guid userId, CancellationToken cancellationToken);

    public Task<AuthenticationResult> GetUserDataAsync(Guid id, CancellationToken cancellationToken);
}