using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Bookings.Features.Payment.Klarna.OrderCallback;

public class OrderCallbackEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/callback", async (HttpContext context, OrderCallbackHandler orderCallbackHandler) =>
        {
            var response = await orderCallbackHandler.Handle(context);

            return Results.Ok(response);
        })
        .AllowAnonymous()
        .Produces<OrderCallbackResponse>(StatusCodes.Status200OK);
    }
}
