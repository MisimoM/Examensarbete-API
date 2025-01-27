using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Data;
using Shared.Exceptions;

namespace Modules.Users.Features.Users.DeleteUser;

public class DeleteUserHandler(UserDbContext dbContext)
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            throw new NotFoundException($"User with the ID '{request.Id}' doesn't exist");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteUserResponse($"User with the ID '{request.Id}' was deleted successfully");
    }
}
