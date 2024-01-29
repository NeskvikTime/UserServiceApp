using UserServiceApp.API.Middleware;

public static class RequestPipeline
{
    public static IApplicationBuilder AddApiMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpRequestMiddleware>();
        return app;
    }
}