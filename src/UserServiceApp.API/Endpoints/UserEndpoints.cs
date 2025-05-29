using MediatR;
using UserServiceApp.API.Endpoints.Users;

namespace UserServiceApp.API.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("v1/users")
                            .WithOpenApi();

        usersGroup.MapGetAllUsersEndpoint();
        usersGroup.MapGetUserEndpoint();
        usersGroup.MapUpdateUserEndpoint();
        usersGroup.MapRegisterUserEndpoint();
        usersGroup.MapLoginEndpoint();
        usersGroup.MapDeleteUserEndpoint();

        return app;
    }
}