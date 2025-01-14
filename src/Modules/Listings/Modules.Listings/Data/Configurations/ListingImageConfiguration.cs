using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Listings.Entities;

namespace Modules.Listings.Data.Configurations;

internal class ListingImageConfiguration : IEntityTypeConfiguration<ListingImage>
{
    public void Configure(EntityTypeBuilder<ListingImage> builder)
    {
        builder.HasKey(li => li.Id);

        builder.Property(li => li.Url)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(li => li.AltText)
               .HasMaxLength(200);

        builder.HasOne(li => li.Listing)
               .WithMany(l => l.Images)
               .HasForeignKey(li => li.ListingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(li => li.ListingId);
    }
}
