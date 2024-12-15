using MediatR;

namespace UserServiceApp.API.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

public record LoginRefreshTokenRequest(string refreshToken);

public sealed class LoginRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/refresh-token", async (string refreshToken, ISender sender) =>
        {
            var result = await sender.Send(new LoginRefreshTokenRequest(refreshToken), CancellationToken.None);

        }).WithTags();
    }
}
