using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UserServiceApp.Tests.Shared.Builders;
using UserServiceApp.Tests.Shared.Common;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.API.IntegrationTests.Controllers;
public class RegisterUserTests : BaseIntegrationTest
{
    private const string UrlPath = "/v1/users/register";

    public RegisterUserTests(ApplicationApiFactory factory) : base(factory)
    {
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
        var response = await _httpClient.PostAsJsonAsync(UrlPath, registerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userResponse = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        userResponse.Should().NotBeNull();
        userResponse!.UserResponse.Should().NotBeNull();
        userResponse!.UserResponse.Email.Should().Be(registerRequest.Email);
        userResponse!.Token.Should().NotBeNullOrWhiteSpace();
        userResponse.UserResponse.FullName.Should().Be(registerRequest.FullName);
        userResponse.UserResponse.Username.Should().Be(registerRequest.UserName);
        userResponse.UserResponse.MobileNumber.Should().Be(registerRequest.MobileNumber);
        userResponse.UserResponse.Email.Should().Be(registerRequest.Email);
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        User existingUser = new UserBuilder()
            .WithUsername("existingUser")
            .WithFullName("Existing User")
            .WithEmail("duplicateemail@example.com")
            .WithMobileNumber("+1234567890")
            .Build();

        string password = "ValidPassword123#";
        string hashedPassword = _passwordHasher.HashPassword(password);
        existingUser.AssignPasswordHash(hashedPassword);

        var cancellationToken = CancellationToken.None;

        await _userRepository.AddUserAsync(existingUser, cancellationToken);

        // Attempt to register another user with the same email
        var duplicateEmailRequest = new RegisterUserRequest(
            "LolekUser",
            "User Name2",
            existingUser.Email, // Duplicate email
            "+0987654321",
            "AnotherValidPassword123#");

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, duplicateEmailRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterUser_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange - Invalid mobile number format
        var invalidDataRequest = new RegisterUserRequest(
            "invalidDataUser",
            "Invalid Data User",
            "invaliduser@example.com",
            "invalidNumber", // Invalid mobile number
            "ValidPassword123#");

        // Act
        var response = await _httpClient.PostAsJsonAsync(UrlPath, invalidDataRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
