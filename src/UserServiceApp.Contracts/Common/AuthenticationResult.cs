using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Contracts.Common;

public record AuthenticationResult
{
    public AuthenticationResult(User user, string token)
    {
        User = user;
        Token = token;
    }

    public User User { get; set; }

    public string Token { get; set; }

    public static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FullName,
            authResult.User.Email,
            authResult.Token);
    }
}