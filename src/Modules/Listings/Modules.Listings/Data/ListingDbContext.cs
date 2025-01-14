using Microsoft.EntityFrameworkCore;
using Modules.Listings.Entities;

namespace Modules.Listings.Data;

internal class ListingDbContext : DbContext
{
    public ListingDbContext(DbContextOptions<ListingDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings { get; set; } = default!;
    public DbSet<ListingImage> ListingImages { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Listings");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListingDbContext).Assembly);
    }
}
