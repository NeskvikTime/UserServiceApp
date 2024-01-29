using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Common.Interfaces;
public interface IUsersRepository
{
    Task AddUserAsync(User user, CancellationToken cancellationToken);

    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);

    Task UpdateAsync(User user, CancellationToken cancellationToken);

    Task DeleteAsync(User user, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);

    Task<bool> EmailIsUniqueAsync(Guid id, string email, CancellationToken cancellationToken);

    Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken);

    Task<bool> UsernameIsUniqueAsync(Guid id, string name, CancellationToken cancellationToken);

    Task<bool> UsernameIsUniqueAsync(string name, CancellationToken cancellationToken);
}
