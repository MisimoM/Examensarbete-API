using Microsoft.EntityFrameworkCore;
using Modules.Bookings.Entities;

namespace Modules.Bookings.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Bookings");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
    }
}
