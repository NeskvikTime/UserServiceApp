using System.Globalization;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.Exceptions;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Services;

internal class UserService(IUsersRepository usersRepository,
    UserPreferences userPreferences,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IUserService
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly UserPreferences _userPreferences = userPreferences;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;


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

    public async Task<List<User>> GetUsersAsync(Guid? userId, CancellationToken cancellationToken)
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

        bool isPasswordValid = user.CheckPassword(password, _passwordHasher);

        if (!isPasswordValid)
        {
            throw new AuthorizationException("Wrong username or password.");
        }

        return user;
    }

    public async Task<User> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        string hashedPassword = _passwordHasher.HashPassword(command.Password);

        var user = new User(
            command.UserName,
            command.FullName,
            command.Email,
            command.MobileNumber,
            _userPreferences.UserCulture,
            _userPreferences.UserLanguage);

        user.AssignPasswordHash(hashedPassword);

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

        string newCulture = request.NewCulture ?? _userPreferences.UserCulture;

        try
        {
            culture = CultureInfo.GetCultureInfo(newCulture);
        }
        catch (CultureNotFoundException)
        {
            culture = CultureInfo.CurrentCulture;
        }

        user.UpdateUserData(
            request.UserName,
            request.FullName,
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
