using Modules.Users.Dtos;
using Modules.Users.Entities;

namespace Modules.Users.Common.Helpers;

public class UserMapper
{
    public static UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}
