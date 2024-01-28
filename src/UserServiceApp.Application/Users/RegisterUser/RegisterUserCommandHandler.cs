using MediatR;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Helper;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.RegisterUser;
public class RegisterUserCommandHandler(IHttpContextAccessor _httpContextAccesor, IUserService _userService, IJwtTokenGenerator _jwtTokenGenerator)
    : IRequestHandler<RegisterUserCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        CultureInfo culture = _httpContextAccesor.GetCultureFromRequest();

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
