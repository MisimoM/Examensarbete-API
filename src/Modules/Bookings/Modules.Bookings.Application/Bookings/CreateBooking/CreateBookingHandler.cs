using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Bookings.Application.Interfaces;
using Modules.Bookings.Domain.Entities;
using Modules.Listings.Communication;
using System.Security.Claims;

namespace Modules.Bookings.Application.Bookings.CreateBooking;

public class CreateBookingHandler(IBookingDbContext dbContext, IListingService listingService, IValidator<CreateBookingRequest> validator, IHttpContextAccessor httpContextAccessor, ILogger<CreateBookingHandler> logger)
{
    private readonly IBookingDbContext _dbContext = dbContext;
    private readonly IListingService _listingService = listingService;
    private readonly IValidator<CreateBookingRequest> _validator = validator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<CreateBookingHandler> _logger = logger;
    public async Task<IResult> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            _logger.LogWarning("Unauthorized access attempt to create booking. HttpContext missing UserId.");
            return Results.Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        _logger.LogInformation("Received booking request from UserId: {UserId} for ListingId: {ListingId}. StartDate: {StartDate}, EndDate: {EndDate}",
            userId, request.ListingId, request.StartDate, request.EndDate);


        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for booking request from UserId: {UserId}. Errors: {Errors}", userId, validationResult.Errors.Select(e => e.ErrorMessage));
            return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var listing = await _listingService.GetByIdAsync(request.ListingId);

        var existingBooking = await _dbContext.Bookings
                .Where(b => b.ListingId == request.ListingId &&
                            b.StartDate < request.EndDate &&
                            b.EndDate > request.StartDate)
                .FirstOrDefaultAsync(cancellationToken);

        if (existingBooking is not null)
        {
            _logger.LogWarning("Booking conflict for ListingId: {ListingId}. StartDate: {StartDate}, EndDate: {EndDate}. ExistingBookingId: {ExistingBookingId}",
                request.ListingId, request.StartDate, request.EndDate, existingBooking.Id);
            return Results.BadRequest("The selected dates are not available for this listing.");
        }

        var booking = Booking.Create(
            userId,
            request.ListingId,
            request.StartDate,
            request.EndDate,
            listing.Price);

        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateBookingResponse(
            booking.Id,
            booking.ListingId,
            booking.StartDate,
            booking.EndDate,
            booking.TotalPrice,
            booking.Status,
            booking.CreatedAt);

        _logger.LogInformation("Booking created successfully. BookingId: {BookingId}, UserId: {UserId}, ListingId: {ListingId}, TotalPrice: {TotalPrice}",
            booking.Id, booking.UserId, booking.ListingId, booking.TotalPrice);


        return Results.Ok(response);
    }
}
