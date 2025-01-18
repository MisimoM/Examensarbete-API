using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Data;
using Modules.Users.Entities;
using Shared.Exceptions;

namespace Modules.Users.Features.Authentication.Login;

internal class LoginHandler(UserDbContext dbContext, IPasswordHasher passwordHasher, ITokenProvider tokenProvider, IValidator<LoginRequest> validator)
{
    private readonly UserDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IValidator<LoginRequest> _validator = validator;

    public async Task<LoginResponse> Handle(LoginRequest request, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
        }

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
            throw new BadRequestException("Invalid login credentials.");

        var accessToken = _tokenProvider.CreateAccessToken(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenProvider.CreateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        CookieFactory.AppendCookie(httpContext.Response, "accessToken", accessToken, TimeSpan.FromMinutes(10));
        CookieFactory.AppendCookie(httpContext.Response, "refreshToken", refreshToken.Token, TimeSpan.FromDays(7));

        return new LoginResponse("Login successful");
    }
}
