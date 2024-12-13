using Modules.Users.Entities;

namespace Modules.Users.Common.Identity;

public interface ITokenProvider
{
    string Create(User user);
}