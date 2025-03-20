using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common.Interfaces;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Infrastructure.Authentication.PasswordHasher;
using UserServiceApp.Infrastructure.Authentication.TokenGenerator;
using UserServiceApp.Infrastructure.Persistence;
using UserServiceApp.Infrastructure.Persistence.Repositories;
using UserServiceApp.Infrastructure.Services;

namespace UserServiceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Section, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddScoped<SaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MyDbContext"))
                .AddInterceptors(serviceProvider.GetRequiredService<SaveChangesInterceptor>());
            options.ConfigureWarnings(warn => warn.Ignore(
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        });

        services.AddScoped<IDbInitializer, ApplicationDbContext>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
