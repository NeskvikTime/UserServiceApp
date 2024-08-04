using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UserServiceApp.Tests.Shared.Common;
using UserServiceApp.Tests.Shared.Extensions;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.IntegrationTests.Controllers;
public class GetAllUserDataTests : BaseIntegrationTest
{
    public GetAllUserDataTests(ApplicationApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAllUserDatas_ReturnsOkWithAllUserResponses()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        // Register multiple users
        var firstUser = await RegisterUserAsync(
            "firstUser@example.com",
            "FirstUserName",
            "First User",
            "+1234567890",
            cancellationToken);

        var secondUser = await RegisterUserAsync(
            "secondUser@example.com",
            "SecondUserName",
            "Second User",
            "+3987654321",
            cancellationToken);

        var getAllUrl = "/v1/users/getAll";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        var getAllResponse = await _httpClient.GetAsync(getAllUrl, cancellationToken);

        // Assert
        getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var usersResponse = await getAllResponse.Content.ReadFromJsonAsync<List<UserResponse>>(cancellationToken);
        usersResponse.Should().NotBeNull();
        usersResponse.Count().Should().BeGreaterOrEqualTo(2);

        usersResponse.Should().ContainSingle(u => u.Id == firstUser.UserResponse.Id);
        usersResponse.Should().ContainSingle(u => u.Id == secondUser.UserResponse.Id);
    }

    [Fact]
    public async Task GetAllUserDatas_WhenNoUsers_ReturnsOkWithEmptyList()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;
        AuthenticationResult authAdminInfo = await base.LoginAdminAsync(cancellationToken);

        var getAllUrl = "/v1/users/getAll";

        // Act
        _httpClient.AddAuthorizationHeader(authAdminInfo.Token);
        var getAllResponse = await _httpClient.GetAsync(getAllUrl, cancellationToken);

        // Assert
        getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var usersResponse = await getAllResponse.Content.ReadFromJsonAsync<List<UserResponse>>(cancellationToken);
        usersResponse.Should().NotBeNull();
        usersResponse!.Count.Should().Be(1); // Only admin user is on the list
    }


    private async Task<AuthenticationResult> RegisterUserAsync(
        string email,
        string userName,
        string fullName,
        string mobileNumber,
        CancellationToken cancellationToken)
    {
        var newUserRequest = new RegisterUserRequest(
            userName,
            fullName,
            email,
            mobileNumber,
            "ValidPassword123#");

        var response = await _httpClient.PostAsJsonAsync("/v1/users/register", newUserRequest, cancellationToken);

        var userResponse = await response.Content.ReadFromJsonAsync<AuthenticationResult>(cancellationToken);
        userResponse.Should().NotBeNull();

        return userResponse!;
    }
}
