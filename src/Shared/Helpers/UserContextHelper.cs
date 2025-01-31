using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.Helpers;

public class UserContextHelper(IHttpContextAccessor httpContextAccessor) : IUserContextHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetUserIdFromClaims()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userIdClaim is null)
            throw new InvalidOperationException("UserId is null in the claims.");

        return Guid.Parse(userIdClaim);
    }
}
