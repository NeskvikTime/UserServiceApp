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

        var id = GetClaimValues("id")
            .Select(Guid.Parse)
            .First();

        var roles = GetClaimValues(ClaimTypes.Role);

        if (roles is null || roles.Count == 0)
        {
            throw new AuthorizationException("User does not have any roles.");
        }

        return new CurrentUser(id, roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        var claims = _httpContextAccessor.HttpContext!.User?.Claims?
            .Where(claim => claim.Type.ToLower() == claimType.ToLower())
            .Select(claim => claim.Value)
            .ToList() ?? [];

        return claims;
    }
}
