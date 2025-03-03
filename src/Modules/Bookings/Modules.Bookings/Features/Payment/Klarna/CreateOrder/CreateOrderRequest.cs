namespace Modules.Bookings.Features.Payment.Klarna.CreateOrder;

public record CreateOrderRequest(Guid BookingId, decimal PricePerNight, int NumberOfNights);
