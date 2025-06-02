using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Contracts.Common;

public record AuthenticationResult
{
    public AuthenticationResult(UserResponse userResponse, string token, string refreshToken = null)
    {
        UserResponse = userResponse;
        Token = token;
        RefreshToken = refreshToken;
    }

    public UserResponse UserResponse { get; set; }

    public string Token { get; set; }

    public string? RefreshToken { get; set; } = null;

    public static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.UserResponse.Id,
            authResult.UserResponse.FullName,
            authResult.UserResponse.Email,
            authResult.Token);
    }
}