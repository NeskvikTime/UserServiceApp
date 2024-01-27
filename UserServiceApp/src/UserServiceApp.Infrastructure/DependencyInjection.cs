using GymManagement.Infrastructure.Authentication.PasswordHasher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Infrastructure.Authentication.TokenGenerator;
using UserServiceApp.Infrastructure.Persistance;
using UserServiceApp.Infrastructure.Persistance.Repositories;
using UserServiceApp.Infrastructure.Services;

namespace UserServiceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Section, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MyDbContext"))
                .AddInterceptors(serviceProvider.GetRequiredService<SaveChangesInterceptor>());
        });

        services.AddScoped<IDbInitializer, ApplicationDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
