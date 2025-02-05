using Modules.Users.Entities;

namespace Modules.Users.Communication;

public interface IUserService
{
    Task<Guid> GetUserIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<User> GetUserAsync(Guid Id, CancellationToken cancellationToken);
}