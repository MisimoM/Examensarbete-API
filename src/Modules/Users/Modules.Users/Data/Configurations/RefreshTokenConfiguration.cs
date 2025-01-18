using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Entities;

namespace Modules.Users.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.HasKey(r => r.Id);

        entity.Property(r => r.Token)
              .HasMaxLength(200);

        entity.HasIndex(r => r.Token)
              .IsUnique();

        entity.HasOne(r => r.User)
              .WithMany()
              .HasForeignKey(r => r.UserId);
    }
}
