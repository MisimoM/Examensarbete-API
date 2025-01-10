namespace Modules.Bookings.Application.Bookings.CreateBooking;

public record CreateBookingRequest(Guid ListingId, DateTime StartDate, DateTime EndDate);
