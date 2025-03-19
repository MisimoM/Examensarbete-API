using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Bookings.Features.Payment.Klarna.GetOrder;

public class GetOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/order/{id}", async (string id, GetOrderHandler getOrderHandler, CancellationToken cancellationToken) =>
        {
            var request = new GetOrderRequest(id);
            var response = await getOrderHandler.Handle(request, cancellationToken);

            return Results.Ok(response);
        })
        .Produces<GetOrderResponse>(StatusCodes.Status200OK);
    }
}
