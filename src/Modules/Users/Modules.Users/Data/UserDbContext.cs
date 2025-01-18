using Microsoft.EntityFrameworkCore;
using Modules.Users.Entities;
namespace Modules.Users.Data;

internal class UserDbContext : DbContext
{

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Users");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

    }
}
