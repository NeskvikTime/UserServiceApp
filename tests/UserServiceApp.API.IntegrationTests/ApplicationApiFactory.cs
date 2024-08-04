using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;
using UserServiceApp.Infrastructure.Persistance;

namespace UserServiceApp.API.IntegrationTests;
public class ApplicationApiFactory : WebApplicationFactory<AssemblyMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Pass@word")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(new ConfigurationBuilder()
            .AddUserSecrets<ApplicationApiFactory>()
            .Build());

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            var descriptorType =
                typeof(DbContextOptions<ApplicationDbContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
        });

        builder.UseEnvironment(Environments.Development);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}

