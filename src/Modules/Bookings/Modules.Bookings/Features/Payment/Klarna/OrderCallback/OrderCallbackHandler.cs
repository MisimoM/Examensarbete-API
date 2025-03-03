using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Modules.Bookings.Features.Payment.Klarna.OrderCallback;

public class OrderCallbackHandler(ILogger<OrderCallbackHandler> logger)
{
    private readonly ILogger<OrderCallbackHandler> _logger = logger;
    public async Task<OrderCallbackResponse> Handle(HttpContext context)
    {

        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation("Header: {Key} = {Value}", header.Key, header.Value);
        }

        return new OrderCallbackResponse("Callback received");
    }
}
