using MediatR;
using UserServiceApp.API.Interfaces;
using UserServiceApp.Application.Users.GetUserDatas;

namespace UserServiceApp.API.Endpoints.Users;

public class GetAllUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("v1/users/get-all", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserDataQuery(null);
            var result = await sender.Send(query, cancellationToken);

            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetAllUsers")
        .WithSummary("Get all user data")
        .WithOpenApi();
    }
}