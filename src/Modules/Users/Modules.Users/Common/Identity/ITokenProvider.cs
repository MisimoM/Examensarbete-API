using Modules.Users.Entities;

namespace Modules.Users.Common.Identity;

internal interface ITokenProvider
{
    string Create(User user);
}