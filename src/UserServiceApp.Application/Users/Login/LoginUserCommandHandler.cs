using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.Login;
public class LoginUserCommandHandler(IUserService _userService, IJwtTokenGenerator _jwtTokenGenerator) : IRequestHandler<LoginUserCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User user = await _userService.LoginUserAsync(command.Email, command.Password, cancellationToken);

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
