using static Modules.Bookings.Features.Payment.Klarna.OrderCallback.OrderCallbackRequest;

namespace Modules.Bookings.Features.Payment.Klarna.OrderCallback;

public record OrderCallbackRequest(
    string OrderId,
    string Status,
    IEnumerable<OrderLine> OrderLines
)
{
    public record OrderLine(string Reference);
}
