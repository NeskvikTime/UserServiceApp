using UserServiceApp.API.Filters;

namespace UserServiceApp.API;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddControllers(cfg =>
        {
            cfg.Filters.Add(typeof(ExceptionFilter));
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();

        return services;
    }
}
