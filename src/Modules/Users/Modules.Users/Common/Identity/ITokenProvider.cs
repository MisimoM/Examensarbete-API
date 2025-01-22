using Modules.Users.Entities;

namespace Modules.Users.Common.Identity;

public interface ITokenProvider
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
}