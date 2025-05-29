using MediatR;
using UserServiceApp.Application.Users.GetUserDatas;

namespace UserServiceApp.API.Endpoints.Users;

public static class GetAllUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetAllUsersEndpoint(this IEndpointRouteBuilder usersGroup)
    {
        usersGroup.MapGet("get-all", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserDataQuery(null);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetAllUsers")
        .WithSummary("Get all user data");
        return usersGroup;
    }
}