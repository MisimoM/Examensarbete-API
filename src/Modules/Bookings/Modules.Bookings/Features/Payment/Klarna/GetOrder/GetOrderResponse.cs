namespace Modules.Bookings.Features.Payment.Klarna.GetOrder;

public record GetOrderResponse(string OrderId, string Status, string HtmlSnippet);
