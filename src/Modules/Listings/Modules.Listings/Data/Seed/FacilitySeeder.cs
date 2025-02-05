using Modules.Listings.Entities;

namespace Modules.Listings.Data.Seed;

internal class FacilitySeeder
{
    public static void Seed(ListingDbContext dbContext)
    {
        if (!dbContext.Facilities.Any())
        {
            var facilities = new List<Facility>
            {
                new("WiFi"),
                new("TV"),
                new("Gratis parkering"),
                new("Kök"),
                new("Tvättmaskin"),
                new("Husdjur tillåtna"),
                new("Öppen spis"),
                new("Cyklar"),
                new("Båt")
            };

            dbContext.Facilities.AddRange(facilities);
            dbContext.SaveChanges();
        }
    }
}
