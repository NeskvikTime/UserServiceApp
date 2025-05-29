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
using Testcontainers.MsSql;
using UserServiceApp.Infrastructure.Persistence;
using Respawn;
using DotNet.Testcontainers.Builders;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Infrastructure.Authentication.PasswordHasher;
using UserServiceApp.Domain.Common.Interfaces;

namespace UserServiceApp.API.IntegrationTests;

public class ApplicationApiFactory : WebApplicationFactory<AssemblyMarker>, IAsyncLifetime
{
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;
    public IUsersRepository UserRepository { get; private set; } = default!;
    public IUnitOfWork UnitOfWork { get; private set; } = default!;
    public IPasswordHasher PasswordHasher { get; private set; } = default!;

    private IServiceScope? _scope;


    private MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Pass@word123!")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithPortBinding(1433, true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddUserSecrets<ApplicationApiFactory>();
        });

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IHostedService>();
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<DbConnection>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = _dbContainer.GetConnectionString();
                options.UseSqlServer(connectionString);
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        HttpClient = CreateClient();

        _scope = Services.CreateScope();
        UserRepository = _scope.ServiceProvider.GetRequiredService<IUsersRepository>();
        UnitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        PasswordHasher = _scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();

        await InitializeRespawner();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    private async Task InitializeRespawner()
    {
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" }
        });
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        if (_dbConnection is not null)
        {
            await _dbConnection.DisposeAsync();
        }
        await _dbContainer.DisposeAsync();
        _scope?.Dispose();
    }
}