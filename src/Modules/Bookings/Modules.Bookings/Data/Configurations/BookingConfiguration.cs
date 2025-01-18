using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Bookings.Entities;

namespace Modules.Bookings.Data.Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.ListingId)
            .IsRequired();

        builder.Property(b => b.StartDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(b => b.EndDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(b => b.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(b => b.UpdatedAt)
            .HasColumnType("datetime");

        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.ListingId);
    }
}
