using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System;
using Testcontainers.MsSql;
using UserServiceApp.Infrastructure.Persistance;
using Respawn;

namespace UserServiceApp.API.IntegrationTests;
public class ApplicationApiFactory : WebApplicationFactory<AssemblyMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Pass@word")
            .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

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
            services.RemoveAll(typeof(IHostedService));
            services.RemoveAll(typeof(ApplicationDbContext));

            services.Remove(services.SingleOrDefault(service
                => typeof(DbContextOptions<ApplicationDbContext>) == service.ServiceType)!);

            services.Remove(services.SingleOrDefault(service
                => typeof(DbConnection) == service.ServiceType)!);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
        });

        builder.UseEnvironment(Environments.Development);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();
        await InitializeRespawner();
    }

    public async Task ResetdatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    private async Task InitializeRespawner()
    {
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}

