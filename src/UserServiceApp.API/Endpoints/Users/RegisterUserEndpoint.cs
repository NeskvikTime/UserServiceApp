using MediatR;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints.Users;

public static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder usersGroup)
    {
        usersGroup.MapPost("register", async (RegisterUserRequest request, ISender sender) =>
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
        .WithSummary("Register a new user");
        return usersGroup;
    }
}