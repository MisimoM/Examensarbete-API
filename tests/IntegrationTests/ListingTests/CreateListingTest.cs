using Microsoft.Extensions.DependencyInjection;
using Modules.Listings.Dtos;
using Modules.Listings.Features.CreateListing;
using Modules.Users.Common.Enums;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Entities;
using System.Net.Http.Json;

namespace IntegrationTests.ListingTests;

public class CreateListingTests : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public CreateListingTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    private string GenerateJwtToken(string username)
    {
        var tokenProvider = _factory.Services.GetRequiredService<ITokenProvider>();
        var passwordHasher = _factory.Services.GetRequiredService<IPasswordHasher>();

        var user = new User
        (
            "Admin User",
            "admin@admin.com",
            UserRole.Admin.ToString(),
            passwordHasher.Hash("Admin123")
        );

        var token = tokenProvider.CreateAccessToken(user);

        return token;
    }

    [Fact]
    public async Task CreateListing_ShouldCreateListing_WhenValidRequest()
    {
        // Arrange
        var request = new CreateListingRequest(
            Title: "Mysigt Hus",
            Description: "Mysigt Hus",
            AccommodationType: "House",
            MainLocation: "Halmstads kommun",
            SubLocation: "Steninge",
            Price: 1500,
            AvailableFrom: DateTime.UtcNow.AddDays(1),
            AvailableUntil: DateTime.UtcNow.AddMonths(1),
            Images: new List<ListingImageDto>
            {
                new ListingImageDto ( Url: "https://example.com/image1.jpg", AltText: "Hus" )
            },
            Facilities: new List<FacilityDto>
            {
                new FacilityDto (1, "WiFi")
            }
        );
        
        var client = _factory.CreateClient();
        var token = GenerateJwtToken("admin");

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await client.PostAsJsonAsync("/listings", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var locationHeader = response.Headers.Location!.ToString();
        Assert.Contains("/listings/", locationHeader);

        var listingId = Guid.Parse(locationHeader.Split("/").Last());
        var dbListing = ListingDbContext.Listings.FirstOrDefault(l => l.Id == listingId);
        Assert.NotNull(dbListing);
        Assert.Equal(request.Title, dbListing?.Title);
        Assert.Equal(request.Description, dbListing?.Description);
    }
}
