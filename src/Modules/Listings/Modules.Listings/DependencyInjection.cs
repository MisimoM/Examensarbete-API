using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Listings.Data;

namespace Modules.Listings;

public static class DependencyInjection
{
    public static IServiceCollection AddListingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ListingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ListingDb")
        ));

        return services;
    }
}
