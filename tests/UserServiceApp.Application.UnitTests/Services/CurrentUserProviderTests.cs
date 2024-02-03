using FluentAssertions;
using System.Security.Claims;
using UserServiceApp.Application.Services;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.Application.UnitTests.Services;
public class CurrentUserProviderTests
{
    [Fact]
    public void GetCurrentUser_WithValidClaims_ReturnsCurrentUser()
    {
        // Arrange
        var userPreferences = new UserPreferences();
        var userId = Guid.NewGuid().ToString();
        var role = "Admin";
        userPreferences.UserClaims = new List<Claim>
        {
            new Claim(type: "id", userId),
            new Claim(ClaimTypes.Role, role)
        };

        var currentUserProvider = new CurrentUserProvider(userPreferences);

        // Act
        var currentUser = currentUserProvider.GetCurrentUser();

        // Assert
        currentUser.Id.Should().Be(Guid.Parse(userId));
        currentUser.Roles.Should().ContainSingle().Which.Should().Be(role);
    }

    [Fact]
    public void GetCurrentUser_WithoutIdClaim_ThrowsAuthorizationException()
    {
        var userId = Guid.NewGuid().ToString();

        // Arrange
        var userPreferences = new UserPreferences
        {
            UserClaims = new List<Claim> { new Claim(type: "id", userId) }
        };

        var currentUserProvider = new CurrentUserProvider(userPreferences);

        // Act
        Action act = () => currentUserProvider.GetCurrentUser();

        // Assert
        act.Should().Throw<AuthorizationException>();
    }

    [Fact]
    public void GetCurrentUser_WithoutIdAnyRoles_ThrowsAuthorizationException()
    {
        // Arrange
        var userPreferences = new UserPreferences
        {
            UserClaims = new List<Claim> { new Claim(ClaimTypes.Role, "Admin") }
        };

        var currentUserProvider = new CurrentUserProvider(userPreferences);

        // Act
        Action act = () => currentUserProvider.GetCurrentUser();

        // Assert
        act.Should().Throw<AuthorizationException>();
    }
}
