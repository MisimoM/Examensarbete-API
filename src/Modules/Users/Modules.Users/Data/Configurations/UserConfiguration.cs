using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Entities;

namespace Modules.Users.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
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

        entity.Property(u => u.ProfileImage)
              .HasMaxLength(500);

        entity.Property(u => u.CreatedAt)
              .IsRequired();
    }
}
