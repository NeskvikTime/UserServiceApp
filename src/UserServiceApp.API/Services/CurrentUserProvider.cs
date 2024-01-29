using System.Globalization;
using System.Security.Claims;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Common.Models;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.API.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    public CultureInfo? UserCulture { get; set; } = null!;

    public ClaimsPrincipal? UserClaims { get; set; } = null!;

    public CurrentUser GetCurrentUser()
    {
        if (UserClaims is null)
        {
            throw new AuthorizationException("Can not read user claims from the request.");
        }

        IReadOnlyList<string> claims = GetClaimValues("id");

        if (claims is null || claims.Count == 0)
        {
            throw new AuthorizationException("Can not read user Id token from the request.");
        }

        Guid? id = Guid.Parse(claims.First());

        if (!id.HasValue)
        {
            throw new AuthorizationException("Can not read user id token from the request.");
        }

        var roles = GetClaimValues(ClaimTypes.Role);

        if (roles is null || roles.Count == 0)
        {
            throw new AuthorizationException("User does not have any roles.");
        }

        return new CurrentUser(id.Value, roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        var claims = UserClaims?.Claims?
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList() ?? [];

        return claims;
    }
}
