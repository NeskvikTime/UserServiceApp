using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace UserServiceApp.Application.Helper;

public static class CultureHelper
{
    public static CultureInfo GetCultureFromRequest(this IHttpContextAccessor accesor)
    {
        var cultures = accesor.HttpContext.Request.Headers.TryGetValue("Accept-Language", out StringValues stringValues);

        string? culture = stringValues.FirstOrDefault();

        if (string.IsNullOrEmpty(culture))
        {
            return CultureInfo.CurrentCulture;
        }

        CultureInfo cultureFromRequest;

        try
        {
            cultureFromRequest = CultureInfo.GetCultureInfo(culture);
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.CurrentCulture;
        }

        return cultureFromRequest;
    }
}
