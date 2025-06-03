using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Tests.Shared.Builders;
using UserServiceApp.Tests.Shared.Extensions;

namespace UserServiceApp.API.IntegrationTests.Endpoints;
public class UpdateUserDataTests : BaseIntegrationTest
{
    public UpdateUserDataTests(ApplicationApiFactory factory) : base(factory)
    { }

    [Fact]
    public async Task UpdateUserData_WithValidData_ReturnsOkWithUpdatedUserResponse()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        // Register a user to update
        var newUserRequest = new RegisterUserRequest(
            "userToUpdate",
            "User To Update",
            "updateuser@example.com",
            "+1234567890",
            "Password123#");

        var registerResponse = await _httpClient.PostAsJsonAsync("/v1/users/register", newUserRequest, cancellationToken);
        var registeredUser = await registerResponse.Content.ReadFromJsonAsync<AuthenticationResult>(cancellationToken);

        var updateRequest = new UpdateUserRequestBuilder()
            .WithUserName("updatedUser")
            .WithFullName("Updated Full Name")
            .WithEmail(newUserRequest.Email)
            .WithMobileNumber("+40721234567")
            .IsAdmin(true)
            .WithNewPassword("NewSecurePassword123!")
            .Build();

        var updateUrl = $"/v1/users/update/{registeredUser!.UserResponse.Id!}";
        string newCulture = "sl-SI";
        string newLanguage = "Slovenian (Slovenia)";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        _httpClient.AddNewUserCultureHeader(newCulture);
        var updateResponse = await _httpClient.PutAsJsonAsync(updateUrl, updateRequest, cancellationToken);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedUserResponse = await updateResponse.Content.ReadFromJsonAsync<UserResponse>(cancellationToken);
        updatedUserResponse.Should().NotBeNull();
        updatedUserResponse!.Email.Should().Be(updateRequest.Email);
        updatedUserResponse.FullName.Should().Be(updateRequest.NewFullName);
        updatedUserResponse.Username.Should().Be(updateRequest.NewUserName);
        updatedUserResponse.MobileNumber.Should().Be(updateRequest.NewMobileNumber);
        updatedUserResponse.IsAdmin.Should().Be(updateRequest.IsAdmin);
        updatedUserResponse.Culture.Should().Be(newCulture);
        updatedUserResponse.Language.Should().Be(newLanguage);
    }

    [Fact]
    public async Task UpdateUserData_NonExistentUser_ReturnsNotFound()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        // Simulate updating a non-existent user
        Guid nonExistentUserId = Guid.NewGuid();
        var updateRequest = new UpdateUserRequestBuilder()
            .WithUserName("nonExistentUser")
            .WithFullName("Non-Existent Full Name")
            .WithEmail("nonexistent@example.com")
            .WithMobileNumber("+40721234567")
            .IsAdmin(false)
            .WithNewPassword("SecurePassword123!")
            .Build();

        var updateUrl = $"/v1/users/update/{nonExistentUserId}";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        _httpClient.AddNewUserCultureHeader("en-US");
        var updateResponse = await _httpClient.PutAsJsonAsync(updateUrl, updateRequest, cancellationToken);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

