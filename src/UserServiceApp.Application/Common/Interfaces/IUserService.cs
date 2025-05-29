using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Common.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);

    Task<List<User>> GetUsersAsync(Guid? userId, CancellationToken cancellationToken);

    Task<User> LoginUserAsync(string email, string password, CancellationToken cancellationToken);

    Task<User> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken);

    Task<User> UpdateUserAsync(UpdateUserCommand request, CancellationToken cancellationToken);
}