namespace UserServiceApp.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<LoggingMiddleware> logger)
    {
        var clientIP = httpContext.Connection.RemoteIpAddress?.ToString();
        var clientName = httpContext?.User?.Identity?.Name;
        var hostName = Environment.MachineName;
        var apiMethodName = $"{httpContext?.Request.Method} {httpContext?.Request.Path}";

        logger.LogInformation("Request: Client IP: {ClientIP}, Client Name: {ClientName}, Host Name: {HostName}, API Method: {ApiMethod}", clientIP, clientName, hostName, apiMethodName);

        await _next(httpContext);
    }
}
