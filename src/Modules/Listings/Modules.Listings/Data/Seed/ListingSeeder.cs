using Modules.Listings.Entities;

namespace Modules.Listings.Data.Seed;

internal class ListingSeeder
{
    public static void Seed(ListingDbContext dbContext)
    {
        if (!dbContext.Listings.Any())
        {
            var listing1Id = Guid.NewGuid();
            var listing2Id = Guid.NewGuid();

            var listing1 = new Listing
            {
                Id = listing1Id,
                HostId = Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                Title = "Liten stuga vid havet",
                Description = "Mysig stuga",
                AccommodationType = "Cottage",
                MainLocation = "Varbergs kommun",
                SubLocation = "Apelviken",
                Price = 1000,
                AvailableFrom = new DateTime(2025, 1, 1),
                AvailableUntil = new DateTime(2026, 12, 31)
            };

            var listing2 = new Listing
            {
                Id = listing2Id,
                HostId = Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                Title = "Mysigt hus i Falkenberg",
                Description = "Mysigt hus",
                AccommodationType = "House",
                MainLocation = "Falkenbergs kommun",
                SubLocation = "Falkenberg stad",
                Price = 1500,
                AvailableFrom = new DateTime(2025, 1, 1),
                AvailableUntil = new DateTime(2026, 12, 31)
            };

            dbContext.Listings.AddRange(listing1, listing2);

            dbContext.ListingImages.AddRange(
                new ListingImage
                {
                    ListingId = listing1Id,
                    Url = "https://github.com/MisimoM/Examensarbete-media/blob/main/Apelviken/apelviken-1.jpg?raw=true",
                    AltText = "Stuga"
                },
                new ListingImage
                {
                    ListingId = listing1Id,
                    Url = "https://github.com/MisimoM/Examensarbete-media/blob/main/Apelviken/apelviken-2.jpg?raw=true",
                    AltText = "Sovrum"
                },
                new ListingImage
                {
                    ListingId = listing2Id,
                    Url = "https://github.com/MisimoM/Examensarbete-media/blob/main/Falkenberg/falkenberg-1.jpg?raw=true",
                    AltText = "Hus"
                },
                new ListingImage
                {
                    ListingId = listing2Id,
                    Url = "https://github.com/MisimoM/Examensarbete-media/blob/main/Falkenberg/falkenberg-2.jpg?raw=true",
                    AltText = "Kök"
                }
            );

            dbContext.SaveChanges();
        }
    }
}
