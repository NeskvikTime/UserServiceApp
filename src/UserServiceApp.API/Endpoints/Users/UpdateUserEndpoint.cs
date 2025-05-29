using MediatR;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Endpoints.Users;

public static class UpdateUserEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder usersGroup)
    {
        usersGroup.MapPut("update/{userId:guid}", async (
            Guid userId,
            UpdateUserRequest request,
            ISender sender,
            HttpContext context) =>
        {
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
        return usersGroup;
    }
}