using System.Globalization;
using UserServiceApp.API.Helper;
using UserServiceApp.Application.Services;

namespace UserServiceApp.API.Middleware;

public class LoggingMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext, UserPreferences userPreferences, ILogger<LoggingMiddleware> logger)
    {
        var clientIP = httpContext.Connection.RemoteIpAddress?.ToString();
        var clientName = httpContext?.User?.Identity?.Name;
        var hostName = Environment.MachineName;
        var apiMethodName = $"{httpContext?.Request.Method} {httpContext?.Request.Path}";

        CultureInfo cultureInfo = httpContext!.Request.Headers.GetCultureFromRequest();

        userPreferences.UserCulture = cultureInfo.Name;
        userPreferences.UserLanguage = cultureInfo.TwoLetterISOLanguageName;
        userPreferences.UserClaims = httpContext.User.Claims;

        logger.LogInformation("Request: Client IP: {ClientIP}, Client Name: {ClientName}, Host Name: {HostName}, API Method: {ApiMethod}", clientIP, clientName, hostName, apiMethodName);

        await _next(httpContext);
    }
}
