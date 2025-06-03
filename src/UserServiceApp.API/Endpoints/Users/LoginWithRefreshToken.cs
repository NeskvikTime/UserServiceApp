using MediatR;
using UserServiceApp.API.Interfaces;
using UserServiceApp.Application.Users.LoginWithRefreshToken;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints.Users;

public class LoginWithRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("v1/users/refresh-token", async (LoginWithRefreshTokenRequest request, ISender sender) =>
        {
            var command = new LoginWithRefreshTokenCommand(request.RefreshToken);
            var result = await sender.Send(command, CancellationToken.None);

            return Results.Ok(result);
        })
        .AllowAnonymous()
        .WithName("LoginWithRefreshToken")
        .WithSummary("Login with refresh token and get new access/refresh tokens")
        .WithOpenApi();
    }
}
