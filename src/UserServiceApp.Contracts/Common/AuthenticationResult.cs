using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Contracts.Common;

public record AuthenticationResult
{
    public AuthenticationResult(UserResponse userResponse, string token, string refreshToken)
    {
        UserResponse = userResponse;
        Token = token;
        RefreshToken = refreshToken;
    }

    public UserResponse UserResponse { get; set; }

    public string Token { get; set; }

    public string RefreshToken { get; set; }
}