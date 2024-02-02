using System.Globalization;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.Exceptions;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Services;

internal class UserService(IUsersRepository _usersRepository,
    UserPreferences _userPreferences,
    IUnitOfWork _unitOfWork,
    IPasswordHasher _passwordHasher) : IUserService
{
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with id {userId} not found");
        }

        await _usersRepository.DeleteAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<User>> GetUserDatasAsync(Guid? userId, CancellationToken cancellationToken)
    {
        List<User> users = new List<User>();

        if (!userId.HasValue)
        {
            users = await _usersRepository.GetAllUsersAsync(cancellationToken);
            return users;
        }

        User? user = await _usersRepository.GetByIdAsync(userId.Value, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with id {userId} not found");
        }

        users.Add(user);
        return users;
    }

    public async Task<User> LoginUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            throw new AuthorizationException("Wrong username or password.");
        }

        bool isPasswordValid = user.CheckPasswordAsync(password, _passwordHasher);

        if (!isPasswordValid)
        {
            throw new AuthorizationException("Wrong username or password.");
        }

        return user;
    }

    public async Task<User> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        string hahsedPassword = _passwordHasher.HashPassword(command.Password);

        var user = new User(
            command.UserName,
            command.FullName,
            command.Email,
            command.MobileNumber,
            _userPreferences.UserCulture,
            _userPreferences.UserLanguage);

        user.AssignHash(hahsedPassword);

        await _usersRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User> UpdateUserAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found");
        }

        CultureInfo culture;

        try
        {
            culture = CultureInfo.GetCultureInfo(request.NewCulture);
        }
        catch (CultureNotFoundException)
        {
            culture = CultureInfo.CurrentCulture;
        }

        user.UpdateUserData(
            request.UserName,
            request.FullName,
            request.Email,
            request.MobileNumber,
            culture.Name,
            culture.EnglishName,
            _passwordHasher,
            request.IsAdmin,
            request.NewPassword);

        await _usersRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}
