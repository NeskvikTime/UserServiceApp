using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.API.IntegrationTests.Controllers;

[Collection("UserCollection")]
public class LoginWithRefreshTokenTests : IAsyncLifetime
{
    private const string LoginUrlPath = "/v1/users/login";
    private const string RefreshTokenUrlPath = "/v1/users/refresh-token";

    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabase;
    private readonly IUsersRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Faker<User> _userFaker;

    public LoginWithRefreshTokenTests(ApplicationApiFactory applicationApiFactory)
    {
        _httpClient = applicationApiFactory.HttpClient;
        _resetDatabase = applicationApiFactory.ResetDatabaseAsync;
        _userRepository = applicationApiFactory.UserRepository;
        _unitOfWork = applicationApiFactory.UnitOfWork;
        _passwordHasher = applicationApiFactory.PasswordHasher;
        _userFaker = new Faker<User>()
            .CustomInstantiator(f => new User(
                f.Person.UserName,
                f.Person.FullName,
                f.Internet.Email(),
                f.Phone.PhoneNumber(),
                f.Random.Word(),
                f.Locale))
            .RuleFor(u => u.Id, f => f.Random.Guid());
    }

    public Task DisposeAsync() => _resetDatabase();
    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task RefreshToken_WithValidRefreshToken_ReturnsNewTokens()
    {
        // Arrange: Register/login user to get refresh token
        string userPassword = "TestPassword123#";
        string hashedPassword = _passwordHasher.HashPassword(userPassword);
        User newUser = _userFaker.Generate();
        newUser.AssignPasswordHash(hashedPassword);
        CancellationToken cancellationToken = CancellationToken.None;
        await _userRepository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        var loginRequest = new LoginRequest(newUser.Email, userPassword);
        var loginResponse = await _httpClient.PostAsJsonAsync(LoginUrlPath, loginRequest, cancellationToken);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthenticationResult>(cancellationToken);
        authResult.Should().NotBeNull();
        authResult!.RefreshToken.Should().NotBeNullOrWhiteSpace();

        // Act: Use refresh token
        var refreshRequest = new LoginWithRefreshTokenRequest(authResult.RefreshToken!);
        var refreshResponse = await _httpClient.PostAsJsonAsync(RefreshTokenUrlPath, refreshRequest, cancellationToken);

        // Assert
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var refreshResult = await refreshResponse.Content.ReadFromJsonAsync<AuthenticationResult>(cancellationToken);
        refreshResult.Should().NotBeNull();
        refreshResult!.Token.Should().NotBeNullOrWhiteSpace();
        refreshResult.RefreshToken.Should().NotBeNullOrWhiteSpace();
        refreshResult.UserResponse.Email.Should().Be(newUser.Email);
    }

    [Fact]
    public async Task RefreshToken_WithInvalidRefreshToken_ReturnsBadRequest()
    {
        // Arrange
        var refreshRequest = new LoginWithRefreshTokenRequest("invalid-refresh-token");

        // Act
        var response = await _httpClient.PostAsJsonAsync(RefreshTokenUrlPath, refreshRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RefreshToken_WithMalformedRequest_ReturnsBadRequest()
    {
        // Arrange: Empty refresh token
        var refreshRequest = new LoginWithRefreshTokenRequest(string.Empty);

        // Act
        var response = await _httpClient.PostAsJsonAsync(RefreshTokenUrlPath, refreshRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
