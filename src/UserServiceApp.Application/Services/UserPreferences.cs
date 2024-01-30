using System.Security.Claims;

namespace UserServiceApp.Application.Services;

public class UserPreferences
{

    public string UserLanguage { get; set; } = default!;

    public string UserCulture { get; set; } = default!;

    public IEnumerable<Claim>? UserClaims { get; set; } = default!;
}
