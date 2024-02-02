using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Infrastructure.Persistance;
using Xunit;

namespace TestCommon.Common;
public abstract class BaseIntegrationTest
    : IClassFixture<ApplicationApiFactory>,
      IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _httpClient;
    protected readonly ISender _sender;
    protected readonly IUsersRepository _userRepository;
    protected readonly IUnitOfWork _unitOfWork;

    internal readonly ApplicationDbContext DbContext;

    public BaseIntegrationTest(ApplicationApiFactory factory)
    {
        _scope = factory.Services.CreateScope();

        _httpClient = factory.CreateClient();

        _sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        _userRepository = _scope.ServiceProvider.GetRequiredService<IUsersRepository>();
        _unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        DbContext = _scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        // Ensure the database is clean before each test
        CleanDatabaseAsync().Wait();
    }


    private async Task CleanDatabaseAsync()
    {
        DbContext.Users.RemoveRange(DbContext.Users);
        await DbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
        _httpClient?.Dispose();
    }
}
