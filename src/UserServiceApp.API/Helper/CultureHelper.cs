using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace UserServiceApp.API.Helper;

public static class CultureHelper
{
    public static CultureInfo GetCultureFromRequest(this HttpRequest request)
    {
        if (!request.Headers.TryGetValue("Accept-Language", out StringValues stringValues))
        {
            throw new CultureNotFoundException("Can not read culture from the request.");
        }

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
