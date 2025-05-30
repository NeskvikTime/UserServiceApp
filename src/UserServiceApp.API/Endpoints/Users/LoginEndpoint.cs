using MediatR;
using UserServiceApp.API.Interfaces;
using UserServiceApp.Application.Users.Login;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints.Users;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("v1/users/login", async (LoginRequest request, ISender sender) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await sender.Send(command, CancellationToken.None);
            return Results.Ok(result);
        })
        .WithGroupName("v1/users")
        .AllowAnonymous()
        .WithName("Login")
        .WithSummary("Login a user");
    }
}