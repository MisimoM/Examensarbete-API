using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Data;
using Modules.Users.Entities;
using Shared.Exceptions;

namespace Modules.Users.Features.Authentication.Refresh;

internal class RefreshHandler(UserDbContext dbContext, ITokenProvider tokenProvider)
{
    private readonly UserDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<RefreshResponse> Handle(HttpContext httpContext)
    {

        if (!httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenFromCookie) || string.IsNullOrEmpty(refreshTokenFromCookie))
            throw new BadRequestException("Refresh token is missing.");

        var refreshToken = await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenFromCookie);

        if (refreshToken is null)
            throw new BadRequestException("Invalid refresh token.");

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

        return new RefreshResponse("Token refreshed successfully");
    }
}
