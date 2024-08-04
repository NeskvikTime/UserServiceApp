using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UserServiceApp.Tests.Shared.Common;
using UserServiceApp.Tests.Shared.Extensions;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.IntegrationTests.Controllers;
public class GetUserDataTests : BaseIntegrationTest
{
    public GetUserDataTests(ApplicationApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetUserData_WithExistingUserId_ReturnsOkWithUserResponse()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        var newUserRequest = new RegisterUserRequest(
            "newUser",
            "New User",
            "newuser@example.com",
            "+1234567890",
            "ValidPassword123#");

        var registerResponse = await _httpClient.PostAsJsonAsync("/v1/users/register", newUserRequest);

        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var registeredUser = await registerResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        var getUrl = $"/v1/users/get/{registeredUser!.UserResponse.Id}";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        var getResponse = await _httpClient.GetAsync(getUrl);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var userResponse = await getResponse.Content.ReadFromJsonAsync<List<UserResponse>>();
        userResponse.Should().NotBeNull();
        userResponse!.Count.Should().Be(1);

        var userFromResponse = userResponse[0];
        userFromResponse!.Email.Should().Be(newUserRequest.Email);
        userFromResponse.FullName.Should().Be(newUserRequest.FullName);
        userFromResponse.Username.Should().Be(newUserRequest.UserName);
        userFromResponse.MobileNumber.Should().Be(newUserRequest.MobileNumber);
    }

    [Fact]
    public async Task GetUserData_NonExistentUser_ReturnsNotFound()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        var nonExistentUserId = Guid.NewGuid();
        var getUrl = $"/v1/users/get/{nonExistentUserId}";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        var getResponse = await _httpClient.GetAsync(getUrl);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
