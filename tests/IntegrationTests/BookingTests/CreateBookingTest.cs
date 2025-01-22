using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Entities;
using Modules.Bookings.Features.CreateBooking;
using Modules.Listings.Entities;
using Modules.Users.Common.Enums;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Entities;
using System.Net.Http.Json;

namespace IntegrationTests.BookingTests;

public class CreateBookingTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public CreateBookingTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    private string GenerateJwtToken(string username)
    {
        var tokenProvider = _factory.Services.GetRequiredService<ITokenProvider>(); 
        var passwordHasher = _factory.Services.GetRequiredService<IPasswordHasher>();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@admin.com",
            Role = UserRole.Admin.ToString(),
            Password = passwordHasher.Hash("Admin123"),
        };

        var token = tokenProvider.CreateAccessToken(user);

        return token;
    }

    [Fact]
    public async Task CreateBooking_ShouldReturn201_WhenBookingIsSuccessful()
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

        var userId = Guid.NewGuid();
        var client = _factory.CreateClient();

        var token = GenerateJwtToken("admin");

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new CreateBookingRequest(listing.Id, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        // Act
        var response = await client.PostAsJsonAsync("/bookings/create", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CreateBookingResponse>();
        Assert.NotNull(result);
        Assert.Equal(listing.Id, result.ListingId);
        Assert.Equal(request.StartDate, result.StartDate);
        Assert.Equal(request.EndDate, result.EndDate);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturn400_WhenValidationFails()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateJwtToken("admin");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Skicka en ogiltig request där ListingId saknas
        var request = new CreateBookingRequest(Guid.Empty, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        // Act
        var response = await client.PostAsJsonAsync("/bookings/create", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturn404_WhenListingNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateJwtToken("admin");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var nonExistentListingId = Guid.NewGuid();

        var request = new CreateBookingRequest(nonExistentListingId, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        // Act
        var response = await client.PostAsJsonAsync("/bookings/create", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturn409_WhenBookingDatesConflict()
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

        var existingBooking = Booking.Create(
            Guid.NewGuid(),
            listing.Id,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(5),
            1500
        );

        BookingDbContext.Bookings.Add(existingBooking);
        await BookingDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var token = GenerateJwtToken("admin");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var request = new CreateBookingRequest(listing.Id, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        // Act
        var response = await client.PostAsJsonAsync("/bookings/create", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
    }
}
