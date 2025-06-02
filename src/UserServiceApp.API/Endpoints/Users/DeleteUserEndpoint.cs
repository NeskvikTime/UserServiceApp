using MediatR;
using UserServiceApp.API.Interfaces;
using UserServiceApp.Application.Users.DeleteUser;

namespace UserServiceApp.API.Endpoints.Users;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("v1/users/delete/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var request = new DeleteUserCommand(userId);
            await sender.Send(request, CancellationToken.None);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteUser")
        .WithSummary("Delete a user")
        .Produces(StatusCodes.Status204NoContent)
        .WithOpenApi();
    }
}