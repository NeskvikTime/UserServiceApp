using MediatR;
using UserServiceApp.API.Interfaces;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints.Users;

public class RegisterUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("v1/users/register", async (RegisterUserRequest request, ISender sender) =>
        {
            var command = new RegisterUserCommand(
                request.UserName,
                request.FullName,
                request.Email,
                request.Password,
                request.MobileNumber);

            var result = await sender.Send(command, CancellationToken.None);
            return Results.Ok(result);
        })
        .AllowAnonymous()
        .WithName("RegisterUser")
        .WithSummary("Register a new user")
        .WithOpenApi();
    }
}