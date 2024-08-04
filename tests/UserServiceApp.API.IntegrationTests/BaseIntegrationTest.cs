using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http.Json;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Infrastructure.Persistance;

namespace UserServiceApp.API.IntegrationTests;
public abstract class BaseIntegrationTest
    : IClassFixture<ApplicationApiFactory>,
      IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly ISender _sender;
    internal readonly ApplicationDbContext _dbContext;
    protected readonly Func<Task> _resetDatabase;
    protected readonly HttpClient _httpClient;

    public BaseIntegrationTest(ApplicationApiFactory factory)
    {
        _scope = factory.Services.CreateScope();

        _sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        _httpClient = factory.CreateClient();

        _dbContext = _scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        _resetDatabase = factory.ResetdatabaseAsync;
    }

    public void Dispose()
    {
        _scope?.Dispose();
        _dbContext?.Dispose();
        _httpClient?.Dispose();
    }

    public async Task<AuthenticationResult> LoginAdminAsync(CancellationToken cancellationToken)
    {
        var loginAdminRequest = new LoginRequest("admin@localhost", "Admin-1234!");

        var loginResponse = await _httpClient.PostAsJsonAsync("/v1/users/login", loginAdminRequest, cancellationToken);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthenticationResult>(cancellationToken);
        authResult.Should().NotBeNull();
        authResult?.Token.Should().NotBeNull();
        authResult?.UserResponse.Email.Should().Be(loginAdminRequest.Email);

        return authResult!;
    }
}
