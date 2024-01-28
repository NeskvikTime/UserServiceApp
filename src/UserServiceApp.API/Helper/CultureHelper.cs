using System.Globalization;

namespace UserServiceApp.API.Helper;

public static class CultureHelper
{
    public static CultureInfo GetCultureFromRequest(this HttpContext context)
    {
        var cultureFromRequest = context.Request.Headers["Accept-Language"].ToString();

        if (string.IsNullOrEmpty(cultureFromRequest))
        {
            return CultureInfo.CurrentCulture;
        }

        CultureInfo culture;

        try
        {
            culture = CultureInfo.GetCultureInfo(cultureFromRequest);
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.CurrentCulture;
        }

        return culture;
    }
}
