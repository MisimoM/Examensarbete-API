using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modules.Users.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Modules.Users.Common.Identity;

internal sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string CreateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
        };

        var TokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        };

        var accessToken = tokenHandler.CreateToken(TokenDescriptor);

        return tokenHandler.WriteToken(accessToken);
    }

    public string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
