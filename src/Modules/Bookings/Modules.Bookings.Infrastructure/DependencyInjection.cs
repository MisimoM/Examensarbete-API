using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Infrastructure.Data;

namespace Modules.Bookings.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingModuleInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BookingDb")
        ));

        return services;
    }
}
