using Microsoft.EntityFrameworkCore;
using Modules.Listings.Entities;

namespace Modules.Listings.Data;

public class ListingDbContext : DbContext
{
    public ListingDbContext(DbContextOptions<ListingDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<ListingImage> ListingImages { get; set; } = null!;
    public DbSet<Facility> Facilities { get; set; } = null!;
    public DbSet<ListingFacility> ListingFacilities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Listings");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListingDbContext).Assembly);
    }
}
