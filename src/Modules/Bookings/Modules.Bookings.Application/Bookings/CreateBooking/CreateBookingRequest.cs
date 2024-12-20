namespace Modules.Bookings.Application.Bookings.CreateBooking;

public record CreateBookingRequest(Guid UserId, Guid ListingId, DateTime StartDate, DateTime EndDate);
