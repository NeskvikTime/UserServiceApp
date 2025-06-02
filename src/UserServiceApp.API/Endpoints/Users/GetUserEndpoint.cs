using MediatR;
using UserServiceApp.API.Interfaces;
using UserServiceApp.Application.Users.GetUserDatas;

namespace UserServiceApp.API.Endpoints.Users;

public class GetUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("v1/users/get/{userId:guid}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserDataQuery(userId);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetUser")
        .WithSummary("Get a specific user's data")
        .WithOpenApi();
    }
}