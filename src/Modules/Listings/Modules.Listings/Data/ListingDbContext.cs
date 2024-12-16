using Microsoft.EntityFrameworkCore;
using Modules.Listings.Entities;

namespace Modules.Listings.Data;

public class ListingDbContext : DbContext
{
    public ListingDbContext(DbContextOptions<ListingDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Listings");

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(l => l.Description)
                  .IsRequired()
                  .HasMaxLength(1000);

            entity.Property(l => l.ImageUrl)
                  .IsRequired();

            entity.Property(l => l.AccommodationType)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(l => l.MainLocation)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(l => l.SubLocation)
                  .HasMaxLength(200);

            entity.Property(l => l.Price)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");

            entity.HasIndex(l => l.MainLocation);

            entity.HasIndex(l => l.SubLocation);
        });
    }
}
