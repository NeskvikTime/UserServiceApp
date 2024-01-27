using System.Security.Claims;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Common.Models;

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

        return new CurrentUser(id, roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return _httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
