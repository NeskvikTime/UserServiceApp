using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserServiceApp.API.ExceptionHandling;
using UserServiceApp.API.Interfaces;
using UserServiceApp.API.Swagger;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Services;

namespace UserServiceApp.API;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        //services.AddControllers(cfg =>
        //{
        //    cfg.Filters.Add(typeof(LogActionParametersFilter));
        //});

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.ConfigureSwaggerGenOptions());
        services.AddAuthorization();

        services.AddEndpoints(typeof(DependencyInjection).Assembly);


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

    private static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
