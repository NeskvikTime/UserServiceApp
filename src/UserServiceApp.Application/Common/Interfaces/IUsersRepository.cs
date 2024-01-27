using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Common.Interfaces;
public interface IUsersRepository
{
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}
