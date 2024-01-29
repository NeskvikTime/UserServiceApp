using UserServiceApp.API.Filters;
using UserServiceApp.API.Services;
using UserServiceApp.API.Swagger;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.API;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddControllers(cfg =>
        {
            cfg.Filters.Add(typeof(ExceptionFilter));
            cfg.Filters.Add<LogActionParametersFilter>();
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options => options.ConfigureSwaggerGenOptions());
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}
