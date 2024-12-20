namespace Modules.Bookings.Application.Bookings.CreateBooking;

public record CreateBookingResponse(
    Guid Id,
    Guid ListingId,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt);
