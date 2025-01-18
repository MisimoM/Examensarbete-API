namespace Modules.Bookings.Features.CreateBooking;

public record CreateBookingRequest(Guid ListingId, DateTime StartDate, DateTime EndDate);
