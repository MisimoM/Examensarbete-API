using Microsoft.Extensions.DependencyInjection;
using Modules.Listings.Entities;
using Modules.Listings.Features.GetListingById;
using Modules.Users.Common.Enums;
using Modules.Users.Common.Helpers;
using Modules.Users.Entities;
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

        using var scope = _factory.Services.CreateScope();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var user = new User(
            Guid.NewGuid(),
            "Test Host",
            "host@example.com",
            UserRole.Host.ToString(),
            passwordHasher.Hash("securepassword")
        );

        UserDbContext.Users.Add(user);
        await UserDbContext.SaveChangesAsync();

        var listing = new Listing
        (
            Guid.NewGuid(),
            user.Id,
            "Mysigt hus",
            "Mysigt hus",
            "House",
            "Halmstads Kommun",
            "Steninge",
            1500,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddMonths(1)
        );

        listing.Images = new List<ListingImage>
        {
            new(listing.Id,"imageUrl.jpg", "house" )
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