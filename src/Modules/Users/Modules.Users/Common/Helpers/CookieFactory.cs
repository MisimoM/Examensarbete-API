using Microsoft.AspNetCore.Http;

namespace Modules.Users.Common.Helpers;

internal static class CookieFactory
{
    public static CookieOptions CreateCookieOptions(TimeSpan expiration, bool isSecure = true)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = isSecure,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.Add(expiration)
        };
    }

    public static void AppendCookie(HttpResponse response, string key, string value, TimeSpan expiration, bool isSecure = true)
    {
        var options = CreateCookieOptions(expiration, isSecure);
        response.Cookies.Append(key, value, options);
    }
}
