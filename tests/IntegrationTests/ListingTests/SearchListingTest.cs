using Modules.Listings.Entities;
using Modules.Listings.Features.SearchListing;
using System.Net.Http.Json;

namespace IntegrationTests.ListingTests;

public class SearchListingTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public SearchListingTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchListing_ShouldReturnListings_WhenFiltersMatch()
    {
        // Arrange
        var listings = new List<Listing>
    {
        new Listing
        {
            Id = Guid.NewGuid(),
            Title = "Mysigt Hus",
            Description = "Mysigt hus",
            AccommodationType = "House",
            MainLocation = "Halmstads kommun",
            SubLocation = "Steninge",
            Price = 1500,
            AvailableFrom = DateTime.UtcNow.AddDays(1),
            AvailableUntil = DateTime.UtcNow.AddMonths(1),
            Images = new List<ListingImage>
            {
                new ListingImage { Url = "https://example.com/image1.jpg", AltText = "Hus" }
            }
        },
        new Listing
        {
            Id = Guid.NewGuid(),
            Title = "Mysig stuga",
            Description = "Mysig stuga",
            AccommodationType = "Cottage",
            MainLocation = "Falkenbergs kommun",
            SubLocation = "Skrea Strand",
            Price = 1200,
            AvailableFrom = DateTime.UtcNow.AddDays(2),
            AvailableUntil = DateTime.UtcNow.AddMonths(2),
            Images = new List<ListingImage>
            {
                new ListingImage { Url = "https://example.com/image2.jpg", AltText = "Stuga" }
            }
        }
    };

        ListingDbContext.Listings.AddRange(listings);
        await ListingDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        // Act
        var url = $"/listings?mainLocation=Halmstads%20kommun&subLocation=Steninge&accommodationType=House";

        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<SearchListingResponse>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Mysigt Hus", result[0].Title);
        Assert.Equal("Steninge", result[0].SubLocation);
        Assert.Equal("House", result[0].AccommodationType);
    }

    [Fact]
    public async Task SearchListing_ShouldReturnEmpty_WhenNoFiltersMatch()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var url = $"/listings?mainLocation=Nonexistent&subLocation=NoMatch&accommodationType=Unknown";

        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<SearchListingResponse>>();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
