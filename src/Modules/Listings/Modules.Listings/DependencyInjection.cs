using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Listings.Communication;
using Modules.Listings.Data;
using Modules.Listings.Data.Seed;
using Modules.Listings.Features.CreateListing;
using Modules.Listings.Features.GetListingById;
using Modules.Listings.Features.GetListing;
using Shared;
using System.Reflection;
using Shared.Contracts;

namespace Modules.Listings;

public static class DependencyInjection
{
    public static IServiceCollection AddListingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ListingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ListingDb")
        ));

        services.AddValidatorsFromAssembly(Assembly.Load("Modules.Listings"));

        services.AddScoped<GetListingHandler>();
        services.AddScoped<GetListingByIdHandler>();
        services.AddScoped<CreateListingHandler>();
        services.AddScoped<IListingService, ListingService>();

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

    public static void MigrateAndSeedListing(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var listingDbContext = scope.ServiceProvider.GetRequiredService<ListingDbContext>();
        listingDbContext.Database.Migrate();
        FacilitySeeder.Seed(listingDbContext);
        ListingSeeder.Seed(listingDbContext);
    }
}
