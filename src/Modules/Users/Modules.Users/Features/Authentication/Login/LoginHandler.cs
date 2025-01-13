using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Data;
using Modules.Users.Entities;

namespace Modules.Users.Features.Authentication.Login;

internal class LoginHandler(UserDbContext dbContext, IPasswordHasher passwordHasher, ITokenProvider tokenProvider, IValidator<LoginRequest> validator, ILogger<LoginHandler> logger)
{
    private readonly UserDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IValidator<LoginRequest> _validator = validator;
    private readonly ILogger<LoginHandler> _logger = logger;

    public async Task<IResult> Handle(LoginRequest request, HttpContext httpContext)
    {
        _logger.LogInformation("Login request received for {Email}", request.Email);
        var validationResult = await _validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Login validation failed for {Email}. Errors: {Errors}", request.Email, validationResult.Errors.Select(e => e.ErrorMessage));
            return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
            return Results.BadRequest(new { error = "Invalid login credentials" });

        var accessToken = _tokenProvider.CreateAccessToken(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenProvider.CreateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        CookieFactory.AppendCookie(httpContext.Response, "accessToken", accessToken, TimeSpan.FromMinutes(10));
        CookieFactory.AppendCookie(httpContext.Response, "refreshToken", refreshToken.Token, TimeSpan.FromDays(7));

        _logger.LogInformation("User {Email} logged in successfully", request.Email);
        _logger.LogInformation("Refresh token created for user {UserId}, expires on {ExpiresOnUtc}", refreshToken.UserId, refreshToken.ExpiresOnUtc);
        
        return Results.Ok(new LoginResponse("Login successful"));
    }
}
