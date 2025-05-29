using MediatR;
using UserServiceApp.Application.Users.Login;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints.Users;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder usersGroup)
    {
        usersGroup.MapPost("login", async (LoginRequest request, ISender sender) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await sender.Send(command, CancellationToken.None);
            return Results.Ok(result);
        })
        .AllowAnonymous()
        .WithName("Login")
        .WithSummary("Login a user");
        return usersGroup;
    }
}