using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserServiceApp.Application.Common.Behaviors;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Services;

namespace UserServiceApp.Application;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
        });

        services.AddScoped<UserPreferences>();
        services.AddScoped<IUserService, UserService>();

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}
