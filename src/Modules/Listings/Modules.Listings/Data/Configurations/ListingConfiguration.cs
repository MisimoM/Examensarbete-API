using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Listings.Entities;

namespace Modules.Listings.Data.Configurations;

internal class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.HostId)
            .IsRequired();

        builder.Property(l => l.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(l => l.Description)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(l => l.AccommodationType)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(l => l.MainLocation)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(l => l.SubLocation)
               .HasMaxLength(200);

        builder.Property(l => l.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(l => l.AvailableFrom)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(l => l.AvailableUntil)
               .IsRequired()
               .HasColumnType("date");

        builder.HasMany(l => l.Images)
               .WithOne(li => li.Listing)
               .HasForeignKey(li => li.ListingId);

        builder.HasMany(l => l.ListingFacilities)
               .WithOne(lf => lf.Listing)
               .HasForeignKey(lf => lf.ListingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => l.MainLocation);

        builder.HasIndex(l => l.SubLocation);
    }
}
