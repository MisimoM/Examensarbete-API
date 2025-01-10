using Modules.Users.Entities;

namespace Modules.Users.Common.Identity;

internal interface ITokenProvider
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
}