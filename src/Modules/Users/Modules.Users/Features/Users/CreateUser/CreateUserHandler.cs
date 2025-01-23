using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Common.Enums;
using Modules.Users.Common.Helpers;
using Modules.Users.Data;
using Modules.Users.Entities;
using Shared.Exceptions;

namespace Modules.Users.Features.Users.CreateUser;

public class CreateUserHandler(UserDbContext dbContext, IPasswordHasher passwordHasher, IValidator<CreateUserRequest> validator)
{
    private readonly UserDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidator<CreateUserRequest> _validator = validator;

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
        }

        var existingEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingEmail is not null)
            throw new ConflictException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Role = UserRole.Customer.ToString(),
            Password = _passwordHasher.Hash(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(user.Id);
    }
}
