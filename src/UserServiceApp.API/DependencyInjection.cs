using UserServiceApp.API.Filters;
using UserServiceApp.API.Swagger;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Services;

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

        services.AddSwaggerGen(options => options.ConfigureSwaggerGenOptions());
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}
