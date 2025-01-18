using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modules.Bookings.Data;
using Modules.Bookings.Entities;
using Modules.Listings.Communication;
using Shared.Exceptions;
using System.Security.Claims;

namespace Modules.Bookings.Features.CreateBooking;

internal class CreateBookingHandler(BookingDbContext dbContext, IListingService listingService, IValidator<CreateBookingRequest> validator, IHttpContextAccessor httpContextAccessor)
{
    private readonly BookingDbContext _dbContext = dbContext;
    private readonly IListingService _listingService = listingService;
    private readonly IValidator<CreateBookingRequest> _validator = validator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public async Task<CreateBookingResponse> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            throw new InvalidOperationException("UserId is null");

        var userId = Guid.Parse(userIdClaim!);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
        }

        var listing = await _listingService.GetByIdAsync(request.ListingId);
        if (listing is null)
            throw new NotFoundException($"Listing with ID {request.ListingId} not found.");

        var existingBooking = await _dbContext.Bookings
                .Where(b => b.ListingId == request.ListingId &&
                            b.StartDate < request.EndDate &&
                            b.EndDate > request.StartDate)
                .FirstOrDefaultAsync(cancellationToken);

        if (existingBooking is not null)
            throw new ConflictException("The selected dates are not available for this listing.");

        var booking = Booking.Create(
            userId,
            request.ListingId,
            request.StartDate,
            request.EndDate,
            listing.Price
        );

        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateBookingResponse(
            booking.Id,
            booking.ListingId,
            booking.StartDate,
            booking.EndDate,
            booking.TotalPrice,
            booking.Status,
            booking.CreatedAt
        );
    }
}
