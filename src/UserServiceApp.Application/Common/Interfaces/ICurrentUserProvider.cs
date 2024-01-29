using System.Globalization;
using System.Security.Claims;
using UserServiceApp.Application.Common.Models;

namespace UserServiceApp.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    CultureInfo? UserCulture { get; set; }

    ClaimsPrincipal? UserClaims { get; set; }

    CurrentUser GetCurrentUser();
}