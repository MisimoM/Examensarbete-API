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
                title: "Strandnära stuga i Apelviken",
                description: "Charmig och bekväm stuga nära havet, perfekt för avkoppling",
                accommodationType: "Stuga",
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
                hostId: Guid.Parse("f2a7e6b3-8c1d-4c5d-a1f6-9e3d5e7b8a2c"),
                title: "Charmigt hus i hjärtat av Falkenberg",
                description: "Hemtrevligt och bekvämt hus med närhet till stad, natur och hav.",
                accommodationType: "Hus",
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

            var listing3 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d9f4b8e2-3a7c-4d5f-8b1e-6c2a3e9d7f5b"),
                title: "Unik fyrbostad på Getterön",
                description: "Bo i en charmig fyr på natursköna Getterön! Njut av havsutsikt, kustnära promenader och en unik boendemiljö med historisk karaktär.",
                accommodationType: "Stuga",
                mainLocation: "Varbergs kommun",
                subLocation: "Getterön",
                price: 1200,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing3.Images = new List<ListingImage>
            {
                new(listing3.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Fyr%20p%C3%A5%20Getter%C3%B6n/Getter%C3%B6n-fyr-1.jpg?raw=true", "Fyr"),
                new(listing3.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Fyr%20p%C3%A5%20Getter%C3%B6n/Getter%C3%B6n-fyr-2.jpg?raw=true", "Dörr"),
                new(listing3.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Fyr%20p%C3%A5%20Getter%C3%B6n/Getter%C3%B6n-fyr-3.jpg?raw=true", "Hav")
            };

            listing3.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing3.Id, FacilityId = 1 },
                new() { ListingId = listing3.Id, FacilityId = 2 },
                new() { ListingId = listing3.Id, FacilityId = 3 },
                new() { ListingId = listing3.Id, FacilityId = 4 },
                new() { ListingId = listing3.Id, FacilityId = 8 },
                new() { ListingId = listing3.Id, FacilityId = 9 },
            };

            var listing4 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("a1c5e7d3-9b2f-4d8c-6f3a-7e5b1d2f9c8a"),
                title: "Charmigt hus i Kungsbacka",
                description: "Trivsamt hus med perfekt läge i Kungsbacka. Njut av närhet till både stadsliv och vacker natur.",
                accommodationType: "Hus",
                mainLocation: "Kungsbacka kommun",
                subLocation: "Kungsbacka stad",
                price: 1700,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing4.Images = new List<ListingImage>
            {
                new(listing4.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Kungsbacka/Kungsbacka-1.jpg?raw=true", "Hus"),
                new(listing4.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Kungsbacka/Kungsbacka-2.jpg?raw=true", "Sovrum")
            };

            listing4.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing4.Id, FacilityId = 1 },
                new() { ListingId = listing4.Id, FacilityId = 2 },
                new() { ListingId = listing4.Id, FacilityId = 3 },
                new() { ListingId = listing4.Id, FacilityId = 4 },
                new() { ListingId = listing4.Id, FacilityId = 5 },
                new() { ListingId = listing4.Id, FacilityId = 6 },
            };

            var listing5 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                title: "Mysig stuga i natursköna Simlångsdalen",
                description: "Lugnt belägen stuga omgiven av vacker natur, perfekt för avkoppling och friluftsliv nära Halmstad",
                accommodationType: "Stuga",
                mainLocation: "Halmstads kommun",
                subLocation: "Simlångsdalen",
                price: 1300,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing5.Images = new List<ListingImage>
            {
                new(listing5.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Siml%C3%A5ngsdalen/Siml%C3%A5ngsdalen-1.jpg?raw=true", "Stuga"),
                new(listing5.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Siml%C3%A5ngsdalen/Siml%C3%A5ngsdalen-3.jpg?raw=true", "Fönster"),
                new(listing5.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Siml%C3%A5ngsdalen/Siml%C3%A5ngsdalen-2.jpg?raw=true", "Båt")
            };

            listing5.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing5.Id, FacilityId = 1 },
                new() { ListingId = listing5.Id, FacilityId = 2 },
                new() { ListingId = listing5.Id, FacilityId = 3 },
                new() { ListingId = listing5.Id, FacilityId = 4 },
                new() { ListingId = listing5.Id, FacilityId = 6 },
                new() { ListingId = listing5.Id, FacilityId = 9 },
            };

            var listing6 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                title: "Trivsam stuga vid sjön i Unnaryd",
                description: "Avkopplande stuga med naturnära läge vid sjö och skog, perfekt för en lugn getaway",
                accommodationType: "Stuga",
                mainLocation: "Hylte kommun",
                subLocation: "Unnaryd",
                price: 900,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing6.Images = new List<ListingImage>
            {
                new(listing6.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Unnaryd/Unnaryd-1.jpg?raw=true", "Stuga"),
                new(listing6.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Unnaryd/Unnaryd-2.jpg?raw=true", "Sjö")
            };

            listing6.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing6.Id, FacilityId = 1 },
                new() { ListingId = listing6.Id, FacilityId = 2 },
                new() { ListingId = listing6.Id, FacilityId = 3 },
                new() { ListingId = listing6.Id, FacilityId = 4 },
                new() { ListingId = listing6.Id, FacilityId = 6 },
                new() { ListingId = listing6.Id, FacilityId = 9 },
            };

            var listing7 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("f2a7e6b3-8c1d-4c5d-a1f6-9e3d5e7b8a2c"),
                title: "Havsnära stuga i Varberg",
                description: "Mysig stuga med vacker havsutsikt och lugnt läge, perfekt för en avkopplande vistelse vid kusten.",
                accommodationType: "Stuga",
                mainLocation: "Varbergs kommun",
                subLocation: "Varberg stad",
                price: 1250,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing7.Images = new List<ListingImage>
            {
                new(listing7.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Varberg-2/Varberg-2-1.jpg?raw=true", "Stuga"),
                new(listing7.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Varberg-2/Varberg-2-2.jpg?raw=true", "Sovrum")
            };

            listing7.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing7.Id, FacilityId = 1 },
                new() { ListingId = listing7.Id, FacilityId = 2 },
                new() { ListingId = listing7.Id, FacilityId = 3 },
                new() { ListingId = listing7.Id, FacilityId = 6 },
                new() { ListingId = listing7.Id, FacilityId = 8 },
                new() { ListingId = listing7.Id, FacilityId = 9 },
            };

            var listing8 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d9f4b8e2-3a7c-4d5f-8b1e-6c2a3e9d7f5b"),
                title: "Charmigt hus i centrala Varberg",
                description: "Trivsamt hus med perfekt läge i hjärtat av Varberg, nära butiker, restauranger och havet",
                accommodationType: "Hus",
                mainLocation: "Varbergs kommun",
                subLocation: "Varberg stad",
                price: 1600,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing8.Images = new List<ListingImage>
            {
                new(listing8.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Varberg/Varberg-1.jpg?raw=true", "Hus"),
            };

            listing8.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing8.Id, FacilityId = 1 },
                new() { ListingId = listing8.Id, FacilityId = 2 },
                new() { ListingId = listing8.Id, FacilityId = 3 },
                new() { ListingId = listing8.Id, FacilityId = 4 },
                new() { ListingId = listing8.Id, FacilityId = 5 },
                new() { ListingId = listing8.Id, FacilityId = 7 },
            };

            var listing9 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("a1c5e7d3-9b2f-4d8c-6f3a-7e5b1d2f9c8a"),
                title: "Hemtrevligt hus i Laholms hjärta",
                description: "Rymligt och välkomnande hus med charmig atmosfär, nära stadens mysiga gränder, caféer och vacker natur.",
                accommodationType: "Hus",
                mainLocation: "Laholms kommun",
                subLocation: "Laholm stad",
                price: 1300,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing9.Images = new List<ListingImage>
            {
                new(listing9.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Laholm/Laholm-1-1.jpg?raw=true", "Hus"),
                new(listing9.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Laholm/Laholm-1-2.jpg?raw=true", "Kök")
            };

            listing9.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing9.Id, FacilityId = 1 },
                new() { ListingId = listing9.Id, FacilityId = 2 },
                new() { ListingId = listing9.Id, FacilityId = 3 },
                new() { ListingId = listing9.Id, FacilityId = 4 },
                new() { ListingId = listing9.Id, FacilityId = 5 },
                new() { ListingId = listing9.Id, FacilityId = 7 },
            };

            var listing10 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                title: "Trivsam stuga vid havet i Onsala",
                description: "Mysig och bekväm stuga med rymliga ytor, belägen nära havet och vackra natursköna omgivningar – perfekt för en avkopplande vistelse i en rofylld miljö",
                accommodationType: "Stuga",
                mainLocation: "Kungsbacka kommun",
                subLocation: "Onsala",
                price: 1500,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing10.Images = new List<ListingImage>
            {
                new(listing10.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Onsala/Onsala-1.jpg?raw=true", "Stuga"),
                new(listing10.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Onsala/Onsala-2.jpg?raw=true", "Hav")
            };

            listing10.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing10.Id, FacilityId = 1 },
                new() { ListingId = listing10.Id, FacilityId = 2 },
                new() { ListingId = listing10.Id, FacilityId = 3 },
                new() { ListingId = listing10.Id, FacilityId = 4 },
                new() { ListingId = listing10.Id, FacilityId = 6 },
                new() { ListingId = listing10.Id, FacilityId = 9 },
            };

            var listing11 = new Listing(
                Guid.NewGuid(),
                hostId: Guid.Parse("f2a7e6b3-8c1d-4c5d-a1f6-9e3d5e7b8a2c"),
                title: "Rymligt hus i havsnära Onsala",
                description: "Trevligt och bekvämt hus med generösa ytor, nära havet och natursköna omgivningar – perfekt för både avkoppling och vardagsliv",
                accommodationType: "Hus",
                mainLocation: "Kungsbacka kommun",
                subLocation: "Onsala",
                price: 1500,
                availableFrom: new DateTime(2025, 1, 1),
                availableUntil: new DateTime(2026, 12, 31)
            );

            listing11.Images = new List<ListingImage>
            {
                new(listing11.Id, "https://github.com/MisimoM/Examensarbete-media/blob/main/Onsala-2/Onsala2-1.jpg?raw=true", "Hus"),
            };

            listing11.ListingFacilities = new List<ListingFacility>
            {
                new() { ListingId = listing11.Id, FacilityId = 1 },
                new() { ListingId = listing11.Id, FacilityId = 2 },
                new() { ListingId = listing11.Id, FacilityId = 3 },
                new() { ListingId = listing11.Id, FacilityId = 4 },
                new() { ListingId = listing11.Id, FacilityId = 5 },
                new() { ListingId = listing11.Id, FacilityId = 6 },
            };

            dbContext.Listings.AddRange(listing1, listing2, listing3, listing4, listing5, listing6, listing7, listing8, listing9, listing10, listing11);
            dbContext.SaveChanges();
        }
    }
}

