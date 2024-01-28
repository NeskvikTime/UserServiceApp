using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.Login;
public class LoginUserCommandHandler(IUserService _userService) : IRequestHandler<LoginUserCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        AuthenticationResult result = await _userService.LoginUserAsync(command.Email, command.Password, cancellationToken);

        return result;
    }
}
