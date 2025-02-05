using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Modules.Listings.Entities;

namespace Modules.Listings.Data.Configurations;

public class ListingFacilityConfiguration : IEntityTypeConfiguration<ListingFacility>
{
    public void Configure(EntityTypeBuilder<ListingFacility> builder)
    {
        builder.HasKey(lf => new { lf.ListingId, lf.FacilityId });

        builder.HasOne(lf => lf.Listing)
            .WithMany(l => l.ListingFacilities)
            .HasForeignKey(lf => lf.ListingId);

        builder.HasOne(lf => lf.Facility)
            .WithMany(f => f.ListingFacilities)
            .HasForeignKey(lf => lf.FacilityId);
    }
}
