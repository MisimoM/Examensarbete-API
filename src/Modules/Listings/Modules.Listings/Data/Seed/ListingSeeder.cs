using Modules.Listings.Entities;

namespace Modules.Listings.Data.Seed;

internal class ListingSeeder
{
    public static void Seed(ListingDbContext dbContext)
    {
        if (!dbContext.Listings.Any())
        {
            var listing1 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                title: "Liten stuga vid havet",
                description: "Mysig stuga",
                accommodationType: "Cottage",
                mainLocation: "Varbergs kommun",
                subLocation: "Apelviken",
                price: 1000,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing1.Images = new List<ListingImage>
            {
                new(listing1.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Apelviken/apelviken-1.jpg?raw=true", "Stuga"),
                new(listing1.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Apelviken/apelviken-2.jpg?raw=true", "Sovrum")
            };

            listing1.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing1.Id, FacilityId = 1 },
                new() { ListingId = listing1.Id, FacilityId = 2 },
                new() { ListingId = listing1.Id, FacilityId = 3 },
                new() { ListingId = listing1.Id, FacilityId = 4 },
                new() { ListingId = listing1.Id, FacilityId = 5 },
                new() { ListingId = listing1.Id, FacilityId = 7 },
            };

            var listing2 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                title: "Mysigt hus i Falkenberg",
                description: "Mysigt hus",
                accommodationType: "House",
                mainLocation: "Falkenbergs kommun",
                subLocation: "Falkenberg stad",
                price: 1500,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing2.Images = new List<ListingImage>
            {
                new(listing2.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Falkenberg/falkenberg-1.jpg?raw=true", "Hus"),
                new(listing2.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Falkenberg/falkenberg-2.jpg?raw=true", "Kök")
            };

            listing2.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing2.Id, FacilityId = 1 },
                new() { ListingId = listing2.Id, FacilityId = 2 },
                new() { ListingId = listing2.Id, FacilityId = 3 },
                new() { ListingId = listing2.Id, FacilityId = 4 },
                new() { ListingId = listing2.Id, FacilityId = 6 },
                new() { ListingId = listing2.Id, FacilityId = 8 }
            };

            dbContext.Listings.AddRange(listing1, listing2);
            dbContext.SaveChanges();
        }
    }
}

