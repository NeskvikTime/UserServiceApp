using MediatR;
using System.Globalization;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.RegisterUser;

public class RegisterUserCommandHandler(IUserService _userService,
    ICurrentUserProvider _currentUserProvider,
    IJwtTokenGenerator _jwtTokenGenerator)
        : IRequestHandler<RegisterUserCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        CultureInfo? culture = _currentUserProvider.UserCulture;

        if (culture is null)
        {
            throw new CultureNotFoundException("Can not read culture from the request.");
        }

        User user = await _userService.RegisterUserAsync(request, culture, cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

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
