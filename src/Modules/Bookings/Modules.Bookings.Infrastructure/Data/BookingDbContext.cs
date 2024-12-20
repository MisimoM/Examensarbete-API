using Microsoft.EntityFrameworkCore;
using Modules.Bookings.Application.Interfaces;
using Modules.Bookings.Domain.Entities;

namespace Modules.Bookings.Infrastructure.Data;

internal class BookingDbContext : DbContext, IBookingDbContext
{
    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; } = default!;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Bookings");

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.UserId)
            .IsRequired();

            entity.Property(b => b.ListingId)
            .IsRequired();

            entity.Property(b => b.StartDate)
            .IsRequired()
            .HasColumnType("date");

            entity.Property(b => b.EndDate)
                .IsRequired()
                .HasColumnType("date");

            entity.Property(b => b.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(b => b.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(b => b.UpdatedAt)
                .HasColumnType("datetime");

            entity.HasIndex(b => b.UserId);
            entity.HasIndex(b => b.ListingId);
        });
    }
}
