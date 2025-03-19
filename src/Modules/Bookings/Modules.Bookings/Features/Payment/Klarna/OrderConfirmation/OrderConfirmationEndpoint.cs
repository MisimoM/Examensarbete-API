using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Bookings.Features.Payment.Klarna.OrderConfirmation;

public class OrderConfirmationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/order/confirmation", async (HttpContext context, OrderConfirmationHandler orderConfirmationHandler, CancellationToken cancellationToken) =>
        {
            var response = await orderConfirmationHandler.Handle(context, cancellationToken);

            return Results.Ok(response);
        })
        .AllowAnonymous()
        .Produces<OrderConfirmationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
