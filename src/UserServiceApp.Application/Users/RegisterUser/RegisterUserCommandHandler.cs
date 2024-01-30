using MediatR;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.RegisterUser;
public class RegisterUserCommandHandler(IUserService _userService, IJwtTokenGenerator _jwtTokenGenerator)
    : IRequestHandler<RegisterUserCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userService.RegisterUserAsync(request, cancellationToken);

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
