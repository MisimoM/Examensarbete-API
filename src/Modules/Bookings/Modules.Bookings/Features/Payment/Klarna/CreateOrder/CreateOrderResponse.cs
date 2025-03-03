namespace Modules.Bookings.Features.Payment.Klarna.CreateOrder;

public record CreateOrderResponse(string OrderId, string Status, string HtmlSnippet);
