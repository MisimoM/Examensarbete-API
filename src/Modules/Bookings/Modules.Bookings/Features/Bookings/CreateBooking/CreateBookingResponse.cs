namespace Modules.Bookings.Features.Bookings.CreateBooking;

public record CreateBookingResponse(
    Guid Id,
    Guid ListingId,
    DateTime StartDate,
    DateTime EndDate,
    decimal PricePerNight,
    int NumberOfNights,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt);
