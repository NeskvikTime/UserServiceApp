using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Tests.Shared.Extensions;

namespace UserServiceApp.API.IntegrationTests.Endpoints;
public class DeleteUserTests : BaseIntegrationTest
{
    public DeleteUserTests(ApplicationApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DeleteUser_WithExistingUserId_ReturnsNoContent()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;

        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        var newUserRequest = new RegisterUserRequest(
            "userToDelete",
            "User To Delete",
            "deleteuser@example.com",
            "+1234567890",
            "Password123#");
        var registerResponse = await _httpClient.PostAsJsonAsync("/v1/users/register", newUserRequest);
        var registeredUser = await registerResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        registeredUser.Should().NotBeNull();
        registeredUser!.UserResponse.Should().NotBeNull();

        var deleteUrl = $"/v1/users/delete/{registeredUser.UserResponse.Id!}";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        var deleteResponse = await _httpClient.DeleteAsync(deleteUrl);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Additional verification: Attempt to get the deleted user
        var getUrl = $"/v1/users/get/{registeredUser.UserResponse.Id}";
        var getResponse = await _httpClient.GetAsync(getUrl);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_WithNonExistentUserId_ReturnsNoContentOrNotFound()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);
        var nonExistentUserId = Guid.NewGuid();
        var deleteUrl = $"/v1/users/delete/{nonExistentUserId}";

        // Act
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authAdminInfo?.Token}");
        var response = await _httpClient.DeleteAsync(deleteUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
