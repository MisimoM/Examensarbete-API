using Modules.Listings.Entities;
using Modules.Listings.Features.GetListingById;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.ListingTests;

public class GetListingByIdTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public GetListingByIdTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetListingById_ShouldReturnListing_WhenIdExists()
    {
        // Arrange
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = "Test Listing",
            Description = "Mysigt Hus",
            AccommodationType = "House",
            MainLocation = "Halmstads Kommun",
            SubLocation = "Steninge",
            Price = 1500,
            AvailableFrom = DateTime.UtcNow.AddDays(1),
            AvailableUntil = DateTime.UtcNow.AddMonths(1),
            Images = new List<ListingImage>
            {
                new ListingImage { Url = "https://example.com/image1.jpg", AltText = "Hus" }
            }
        };

        ListingDbContext.Listings.Add(listing);
        await ListingDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/listings/{listing.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GetListingByIdResponse>();

        Assert.NotNull(result);
        Assert.Equal(listing.Id, result.Id);
        Assert.Equal(listing.Title, result.Title);
        Assert.Equal(listing.Description, result.Description);
        Assert.Single(result.Images);
        Assert.Equal(listing.Images.First().Url, result.Images.First().Url);
        Assert.Equal(listing.Images.First().AltText, result.Images.First().AltText);
    }

    [Fact]
    public async Task GetListingById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/listings/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
