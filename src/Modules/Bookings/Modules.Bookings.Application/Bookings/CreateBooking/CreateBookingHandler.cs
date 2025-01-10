﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modules.Bookings.Application.Interfaces;
using Modules.Bookings.Domain.Entities;
using Modules.Listings.Communication;
using System.Security.Claims;

namespace Modules.Bookings.Application.Bookings.CreateBooking;

public class CreateBookingHandler(IBookingDbContext dbContext, IListingService listingService, IValidator<CreateBookingRequest> validator, IHttpContextAccessor httpContextAccessor)
{
    private readonly IBookingDbContext _dbContext = dbContext;
    private readonly IListingService _listingService = listingService;
    private readonly IValidator<CreateBookingRequest> _validator = validator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public async Task<IResult> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Results.Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });


        var listing = await _listingService.GetByIdAsync(request.ListingId);

        var existingBooking = await _dbContext.Bookings
                .Where(b => b.ListingId == request.ListingId &&
                            b.StartDate < request.EndDate &&
                            b.EndDate > request.StartDate)
                .FirstOrDefaultAsync(cancellationToken);

        if (existingBooking is not null)
            return Results.BadRequest("The selected dates are not available for this listing.");

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

        return Results.Ok(response);
    }
}
