using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Bookings.Features.Payment.Klarna.CreateOrder;

public class CreateOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/order", async (CreateOrderRequest request, CreateOrderHandler createOrderHandler) =>
        {
            var response = await createOrderHandler.Handle(request);

            return Results.Ok(response);
        })
        .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
