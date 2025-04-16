using Shared.Dtos;

namespace Shared.Contracts;

public interface IUserService
{
    Task<UserDto> GetUserAsync(Guid Id, CancellationToken cancellationToken);
}