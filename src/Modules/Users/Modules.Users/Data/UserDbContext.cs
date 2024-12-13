using Microsoft.EntityFrameworkCore;
using Modules.Users.Entities;
using System.Data;
namespace Modules.Users.Data;

public class UserDbContext : DbContext
{

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = default!;

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
    }
}
