using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Data;
using Modules.Users.Entities;

namespace Modules.Users.Features.Authentication.Refresh;

internal class RefreshHandler(UserDbContext dbContext, ITokenProvider tokenProvider, ILogger<RefreshHandler> logger)
{
    private readonly UserDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ILogger<RefreshHandler> _logger = logger;

    public async Task<IResult> Handle(HttpContext httpContext)
    {
        _logger.LogInformation("Received token refresh request");

        if (!httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenFromCookie) || string.IsNullOrEmpty(refreshTokenFromCookie))
        {
            _logger.LogWarning("Refresh token is missing in the request");
            return Results.BadRequest(new { error = "Refresh token is missing" });
        }

        var refreshToken = await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenFromCookie);

        if (refreshToken is null)
        {
            _logger.LogWarning("Invalid refresh token provided");
            return Results.BadRequest(new { error = "Invalid refresh token" });
        }

        if (refreshToken.ExpiresOnUtc <= DateTime.UtcNow)
        {
            _logger.LogInformation("Refresh token expired for UserId: {UserId}", refreshToken.UserId);
            _dbContext.RefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
            return Results.BadRequest(new { error = "Refresh token has expired" });
        }

        var newAccessToken = _tokenProvider.CreateAccessToken(refreshToken.User);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = refreshToken.UserId,
            Token = _tokenProvider.CreateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _dbContext.RefreshTokens.Remove(refreshToken);
        _dbContext.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync();

        CookieFactory.AppendCookie(httpContext.Response, "accessToken", newAccessToken, TimeSpan.FromMinutes(10));
        CookieFactory.AppendCookie(httpContext.Response, "refreshToken", newRefreshToken.Token, TimeSpan.FromDays(7));

        _logger.LogInformation("Tokens refreshed successfully for UserId: {UserId}", newRefreshToken.UserId);
        return Results.Ok(new RefreshResponse("Token refreshed successfully"));
    }
}
