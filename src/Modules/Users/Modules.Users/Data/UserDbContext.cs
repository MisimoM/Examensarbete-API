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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.HasIndex(u => u.Email)
                  .IsUnique();

            entity.Property(u => u.Password)
                  .IsRequired();
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(r  => r.Id);

            entity.Property(r => r.Token)
                  .HasMaxLength(200);

            entity.HasIndex(r => r.Token)
                  .IsUnique();

            entity.HasOne(r => r.User)
                  .WithMany()
                  .HasForeignKey(r => r.UserId);
        });
    }
}
