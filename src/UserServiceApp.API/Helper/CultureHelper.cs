using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace UserServiceApp.API.Helper;

public static class CultureHelper
{
    public static CultureInfo GetCultureFromRequest(this IHeaderDictionary headers)
    {
        if (headers.TryGetValue("Accept-Language", out StringValues stringValues) || stringValues.Count == 0)
        {
            return CultureInfo.CurrentCulture;
        }

        string? culture = stringValues.First();

        if (string.IsNullOrWhiteSpace(culture))
        {
            return CultureInfo.CurrentCulture;
        }

        try
        {
            return CultureInfo.GetCultureInfo(culture);
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.CurrentCulture;
        }
    }
}
