using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Common.Helpers;
using Modules.Users.Data;
using Shared.Exceptions;

namespace Modules.Users.Features.Users.UpdateUser;

public class UpdateUserHandler(UserDbContext dbContext, IPasswordHasher passwordHasher, IValidator<UpdateUserRequest> validator)
{
    private readonly UserDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidator<UpdateUserRequest> _validator = validator;

    public async Task<UpdateUserResponse> Handle(Guid Id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);

        if (user is null)
            throw new NotFoundException($"User with ID {Id} not found.");

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
        }

        var emailExists = await _dbContext.Users.AnyAsync(u => u.Email == request.Email && u.Id != user.Id, cancellationToken);

        if (emailExists)
            throw new ConflictException($"The email address '{request.Email}' is already in use.");

        user.Name = request.Name;
        user.Email = request.Email;
        user.Password = _passwordHasher.Hash(request.Password);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateUserResponse(user.Id);
    }
}
