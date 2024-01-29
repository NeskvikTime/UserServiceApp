using System.Security.Claims;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Common.Models;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.API.Services;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new ArgumentNullException(nameof(HttpContext));
        }

        IReadOnlyList<string> claimValues = GetClaimValues("id");

        if (claimValues.Count == 0)
        {
            throw new AuthorizationException("User does not have an id assigned to token.");
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
        var claims = _httpContextAccessor.HttpContext!.User?.Claims?
            .Where(claim => string.Equals(claim.Type, claimType, StringComparison.OrdinalIgnoreCase))
            .Select(claim => claim.Value)
            .ToList() ?? [];

        return claims;
    }
}
