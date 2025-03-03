namespace Modules.Bookings.Features.Bookings.CreateBooking;

public record CreateBookingRequest(Guid ListingId, DateTime StartDate, DateTime EndDate);
