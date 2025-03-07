﻿using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Entities;
using Modules.Bookings.Features.Bookings.CreateBooking;
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
    public async Task CreateBooking_ShouldReturn201_WhenBookingIsSuccessful()
    {
        // Arrange
        var listing = new Listing
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Mysigt hus",
            "Mysigt hus",
            "House",
            "Halmstads Kommun",
            "Steninge",
            1500,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddMonths(1)
        );

        ListingDbContext.Listings.Add(listing);
        await ListingDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var token = GenerateJwtToken("admin");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new CreateBookingRequest(listing.Id, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        // Act
        var response = await client.PostAsJsonAsync("/bookings", request);

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

        var request = new CreateBookingRequest(Guid.Empty, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5));

        // Act
        var response = await client.PostAsJsonAsync("/bookings", request);

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
        var response = await client.PostAsJsonAsync("/bookings", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturn409_WhenBookingDatesConflict()
    {
        // Arrange
        var listing = new Listing
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Mysigt hus",
            "Mysigt hus",
            "House",
            "Halmstads Kommun",
            "Steninge",
            1500,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddMonths(1)
        );

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
        var response = await client.PostAsJsonAsync("/bookings", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
    }
}
