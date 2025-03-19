using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Modules.Bookings.Common.Helpers;

internal class KlarnaAuthHelper
{
    public static AuthenticationHeaderValue GetAuthHeader(IConfiguration configuration)
    {
        var username = configuration["Klarna:Username"]!;
        var password = configuration["Klarna:Password"]!;

        var credentials = Encoding.ASCII.GetBytes($"{username}:{password}");
        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
    }
}
