using MediatR;
using UserServiceApp.Application.Users.DeleteUser;

namespace UserServiceApp.API.Endpoints.Users;

public static class DeleteUserEndpoint
{
    public static IEndpointRouteBuilder MapDeleteUserEndpoint(this IEndpointRouteBuilder usersGroup)
    {
        usersGroup.MapDelete("delete/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var request = new DeleteUserCommand(userId);
            await sender.Send(request, CancellationToken.None);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteUser")
        .WithSummary("Delete a user");
        return usersGroup;
    }
}