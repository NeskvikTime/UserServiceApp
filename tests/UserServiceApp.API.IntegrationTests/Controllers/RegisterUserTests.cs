﻿using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.IntegrationTests.Endpoints;

public class RegisterUserTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string UrlPath = "/v1/users/register";
    private readonly Func<RegisterUserRequest> _generateRegisterUserRequest;
    private readonly ApplicationApiFactory _factory;

    public RegisterUserTests(ApplicationApiFactory factory) : base(factory)
    {
        _factory = factory;
        _generateRegisterUserRequest = () =>
        {
            var faker = new Faker();
            return new RegisterUserRequest(
                faker.Internet.UserName(),
                faker.Name.FullName(),
                faker.Internet.Email(),
                $"+{faker.Random.Number(1, 9)}{faker.Random.String2(11, "0123456789")}",
                GenerateValidPassword(faker)
            );
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    private string GenerateValidPassword(Faker faker)
    {
        return faker.Internet.Password(9, true, @"[a-z]") // Ensures lowercase
            + faker.Internet.Password(1, false, @"[A-Z]") // Ensures uppercase
            + faker.Internet.Password(1, false, @"[0-9]") // Ensures number
            + faker.Internet.Password(1, false, @"[!@#$%^&*(),.?\"":{}|<>]"); // Ensures symbol
    }

    [Fact]
    public async Task RegisterUser_WithValidData_ReturnsOkWithUserResponse()
    {
        // Arrange
        var registerRequest = new RegisterUserRequest(
            "newUser",
            "New User",
            "newuser@example.com",
            "+1234567890",
            "ValidPassword123#");

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, registerRequest, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userResponse = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        userResponse.Should().NotBeNull();
        userResponse!.UserResponse.Should().NotBeNull();
        userResponse.UserResponse.Email.Should().Be(registerRequest.Email);
        userResponse.Token.Should().NotBeNullOrWhiteSpace();
        userResponse.UserResponse.FullName.Should().Be(registerRequest.FullName);
        userResponse.UserResponse.Username.Should().Be(registerRequest.UserName);
        userResponse.UserResponse.MobileNumber.Should().Be(registerRequest.MobileNumber);
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ReturnsBadRequest()
    {
        var existingUser = _generateRegisterUserRequest();
        var firstResponse = await _httpClient.PostAsJsonAsync(UrlPath, existingUser, CancellationToken.None);
        firstResponse.EnsureSuccessStatusCode();

        var duplicateEmailRequest = _generateRegisterUserRequest();
        duplicateEmailRequest = duplicateEmailRequest with { Email = existingUser.Email };

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, duplicateEmailRequest, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterUser_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange - Invalid mobile number format
        var invalidDataRequest = _generateRegisterUserRequest();
        invalidDataRequest = invalidDataRequest with { MobileNumber = "invalidNumber" };

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, invalidDataRequest, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}