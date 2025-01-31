using Microsoft.EntityFrameworkCore;
using Modules.Users.Data;

namespace Modules.Users.Communication;

internal class UserService(UserDbContext dbContext) : IUserService
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task<Guid> GetUserIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var userId = await _dbContext.Users
            .Where(u => u.Id == Id)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return userId;
    }
}
