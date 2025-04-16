using Microsoft.EntityFrameworkCore;
using Modules.Users.Data;
using Shared.Contracts;
using Shared.Dtos;

namespace Modules.Users.Communication;

internal class UserService(UserDbContext dbContext) : IUserService
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task<UserDto> GetUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
            return null!;

        return new UserDto(
        user.Name,
        user.Email,
        user.ProfileImage
        );
    }
}
