using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application;

internal class UserService(IUsersRepository _userRepository,
    IUnitOfWork _unitOfWork,
    IPasswordHasher _passwordHasher,
    IJwtTokenGenerator _jwtTokenGenerator) : IUserService
{
    public async Task DeleteUeserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found");
        }

        await _userRepository.DeleteAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllUserDatasAsync(CancellationToken cancellationToken)
    {
        List<User> users = await _userRepository.GetAllUsersAsync(cancellationToken);

        return users;
    }

    public async Task<AuthenticationResult> LoginUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with email {email} not found");
        }

        bool isPasswordValid = user.CheckPasswordAsync(password, _passwordHasher);

        if (!isPasswordValid)
        {
            throw new ArgumentException("Invalid password");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        var userResponse = new UserResponse(
            user.Id,
            user.Username,
            user.FullName,
            user.Email,
            user.MobileNumber,
            user.Language,
            user.Culture,
            user.IsAdmin);

        return new AuthenticationResult(userResponse, token);

    }
}
