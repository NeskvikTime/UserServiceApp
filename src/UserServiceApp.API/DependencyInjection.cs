using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using UserServiceApp.API.ExceptionHandling;
using UserServiceApp.API.Filters;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Services;

namespace UserServiceApp.API;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddControllers(cfg =>
        {
            cfg.Filters.Add(typeof(LogActionParametersFilter));
        });

        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        //services.AddSwaggerGen(options => options.ConfigureSwaggerGenOptions());

        // Add ProblemDetails and custom exception handler
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}
