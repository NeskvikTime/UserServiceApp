using System.Security.Claims;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Common.Models;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.Application.Services;

public class CurrentUserProvider(UserPreferences _userPreferences) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        IReadOnlyList<string> claimValues = GetClaimValues("id");

        if (claimValues.Count == 0)
        {
            throw new AuthorizationException("Bearer token is not valid.");
        }

        string id = claimValues.First();

        var roles = GetClaimValues(ClaimTypes.Role);

        if (roles is null || roles.Count == 0)
        {
            throw new AuthorizationException("User does not have any roles.");
        }

        return new CurrentUser(Guid.Parse(id), roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        var claims = _userPreferences.UserClaims?
            .Where(claim => string.Equals(claim.Type, claimType, StringComparison.OrdinalIgnoreCase))
            .Select(claim => claim.Value)
            .ToList() ?? [];

        return claims;
    }
}
