using UserServiceApp.API.Helper;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.API.Middleware;

public class HttpRequestMiddleware(RequestDelegate _next, ICurrentUserProvider _currentUserProvider, ILogger<HttpRequestMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Get Claims from the request
        if (context.User is not null)
        {
            _currentUserProvider.UserClaims = context.User;
        }

        // Get Culture from the request
        if (context.Request is not null)
        {
            _currentUserProvider.UserCulture = context.Request.GetCultureFromRequest();
        }

        await _next(context);
    }
}
