﻿using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TestCommon.Builders;
using TestCommon.Common;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.API.IntegrationTests.Controllers;
public class LoginAsyncTests : BaseIntegrationTest
{
    public LoginAsyncTests(ApplicationApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkWithUserResponse()
    {
        // Arrange
        string userPasswor = "TestPassword123#";
        string hashedPassword = _passwordHasher.HashPassword(userPasswor);

        User newUser = new UserBuilder()
            .WithUsername("testUser")
            .WithFullName("Test User")
            .WithEmail("test@example.com")
            .WithMobileNumber("+1234567890")
            .WithLanguage("English")
            .WithCulture("en-US")
            .Build();

        newUser.AssignHash(hashedPassword);

        CancellationToken cancellationToken = CancellationToken.None;

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        var loginRequest = new LoginRequest(newUser.Email, userPasswor);

        _dbContext.Users.Should().ContainEquivalentOf(newUser);

        string urlPath = "/v1/users/login";

        // Act
        var response = await _httpClient.PostAsJsonAsync(urlPath, loginRequest, cancellationToken);

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
        User newUser = new UserBuilder()
            .WithUsername("invalidCredsUser")
            .WithFullName("Invalid User")
            .WithEmail("invalid@example.com")
            .WithMobileNumber("+19876543210")
            .WithLanguage("English")
            .WithCulture("en-US")
            .Build();

        string userPasswor = "TestPassword123#";
        string wrongPassword = "WrongPassword123#";

        newUser.AssignHash(_passwordHasher.HashPassword(userPasswor));

        await _userRepository.AddAsync(newUser, CancellationToken.None);
        await _unitOfWork.SaveChangesAsync();

        var loginRequest = new LoginRequest(newUser.Email, wrongPassword);

        string urlPath = "/v1/users/login";

        // Act
        var response = await _httpClient.PostAsJsonAsync(urlPath, loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_NonExistentUser_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest("nonexistent@example.com", "AnyPassword123!");

        // Act
        var response = await _httpClient.PostAsJsonAsync("/v1/users/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithMalformedRequest_ReturnsUnauthorized()
    {
        // Arrange - Example with missing password field
        var loginRequest = new LoginRequest("nonexistent@example.com", string.Empty);


        // Act
        var response = await _httpClient.PostAsJsonAsync("/v1/users/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
