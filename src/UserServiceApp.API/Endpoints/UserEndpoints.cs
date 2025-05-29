using MediatR;
using UserServiceApp.Application.Users.DeleteUser;
using UserServiceApp.Application.Users.GetUserDatas;
using UserServiceApp.Application.Users.Login;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("v1/users")
                            .WithOpenApi();

        // Get all users endpoint
        usersGroup.MapGet("get-all", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserDataQuery(null);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetAllUsers")
        .WithSummary("Get all user data");

        // Get specific user endpoint
        usersGroup.MapGet("get/{userId:guid}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserDataQuery(userId);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetUser")
        .WithSummary("Get a specific user's data");

        // Update user endpoint
        usersGroup.MapPut("update/{userId:guid}", async (
            Guid userId,
            UpdateUserRequest request,
            ISender sender,
            HttpContext context) =>
        {
            // Extract NewUserCulture from header if it exists
            string? newUserCulture = null;
            if (context.Request.Headers.TryGetValue("NewUserCulture", out var cultureValues) && cultureValues.Count > 0)
            {
                newUserCulture = cultureValues.First();
            }

            var command = new UpdateUserCommand(
                userId,
                request.NewUserName,
                request.NewFullName,
                request.Email,
                request.NewMobileNumber,
                request.IsAdmin,
                request.NewPassword,
                newUserCulture);

            var result = await sender.Send(command, CancellationToken.None);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateUser")
        .WithSummary("Update a user's data");

        // Register user endpoint
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

        // Login endpoint
        usersGroup.MapPost("login", async (LoginRequest request, ISender sender) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await sender.Send(command, CancellationToken.None);
            return Results.Ok(result);
        })
        .AllowAnonymous()
        .WithName("Login")
        .WithSummary("Login a user");

        // Delete user endpoint
        usersGroup.MapDelete("delete/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var request = new DeleteUserCommand(userId);
            await sender.Send(request, CancellationToken.None);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteUser")
        .WithSummary("Delete a user");

        return app;
    }
}