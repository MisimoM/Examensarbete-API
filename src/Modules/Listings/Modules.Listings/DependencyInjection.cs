using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Listings.Data;
using Modules.Listings.Features.SearchListing;
using Shared;

namespace Modules.Listings;

public static class DependencyInjection
{
    public static IServiceCollection AddListingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ListingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ListingDb")
        ));

        services.AddScoped<SearchListingHandler>();

        return services;
    }

    public static WebApplication MapListingEndpoints(this WebApplication app)
    {

        var endpointTypes = typeof(DependencyInjection).Assembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var endpointType in endpointTypes)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(endpointType)!;
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
