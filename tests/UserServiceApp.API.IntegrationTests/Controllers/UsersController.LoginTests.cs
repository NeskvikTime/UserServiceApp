using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UserServiceApp.Tests.Shared.Builders;
using UserServiceApp.Tests.Shared.Common;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;
using UserServiceApp.Infrastructure.Authentication.PasswordHasher;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.Common.Interfaces;
using Bogus;

namespace UserServiceApp.API.IntegrationTests.Controllers;

[Collection("UserCollection")]
public class LoginAsyncTests : IAsyncLifetime
{
    private const string UrlPath = "/v1/users/login";

    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabase;

    private readonly IUsersRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IPasswordHasher _passwordHasher;

    private readonly Faker<User> _userFaker;

    public LoginAsyncTests(ApplicationApiFactory applicationApiFactory)
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
    public async Task Login_WithValidCredentials_ReturnsOkWithUserResponse()
    {
        // Arrange
        string userPassword = "TestPassword123#";
        string hashedPassword = _passwordHasher.HashPassword(userPassword);

        User newUser = _userFaker.Generate();
        newUser.AssignPasswordHash(hashedPassword);

        CancellationToken cancellationToken = CancellationToken.None;

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        var loginRequest = new LoginRequest(newUser.Email, userPassword);

        var newUserFromRepo = await _userRepository.GetByIdAsync(newUser.Id, cancellationToken);

        newUserFromRepo.Should().NotBeNull();
        newUserFromRepo.Should().BeEquivalentTo(newUser);

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, loginRequest, cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userResponse = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        userResponse.Should().NotBeNull();
        userResponse?.UserResponse.Email.Should().Be(newUser.Email);
        userResponse?.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        string correctPassword = "TestPassword123#";
        string wrongPassword = "WrongPassword123#";
        string hashedPassword = _passwordHasher.HashPassword(correctPassword);

        User newUser = _userFaker.Generate();
        newUser.AssignPasswordHash(hashedPassword);

        CancellationToken cancellationToken = CancellationToken.None;

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        var loginRequest = new LoginRequest(newUser.Email, wrongPassword);

        var newUserFromRepo = await _userRepository.GetByIdAsync(newUser.Id, cancellationToken);

        newUserFromRepo.Should().NotBeNull();
        newUserFromRepo.Should().BeEquivalentTo(newUser);

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, loginRequest, cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_NonExistentUser_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest("nonexistent@example.com", "AnyPassword123!");

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithMalformedRequest_BadRequest()
    {
        // Arrange - Example with missing password field
        var loginRequest = new LoginRequest("nonexistent@example.com", string.Empty);

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
