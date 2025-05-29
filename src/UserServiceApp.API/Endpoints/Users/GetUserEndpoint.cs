using MediatR;
using UserServiceApp.Application.Users.GetUserDatas;

namespace UserServiceApp.API.Endpoints.Users;

public static class GetUserEndpoint
{
    public static IEndpointRouteBuilder MapGetUserEndpoint(this IEndpointRouteBuilder usersGroup)
    {
        usersGroup.MapGet("get/{userId:guid}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserDataQuery(userId);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetUser")
        .WithSummary("Get a specific user's data");
        return usersGroup;
    }
}