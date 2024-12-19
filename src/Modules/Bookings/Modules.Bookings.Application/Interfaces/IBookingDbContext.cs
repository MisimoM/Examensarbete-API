using Microsoft.EntityFrameworkCore;
using Modules.Bookings.Domain.Entities;

namespace Modules.Bookings.Application.Interfaces;

public interface IBookingDbContext
{
    DbSet<Booking> Bookings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
