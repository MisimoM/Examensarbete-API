using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Bookings.Features.Bookings.CreateBooking;

public class CreateBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/bookings", async (CreateBookingRequest request, CreateBookingHandler createBookingHandler, CancellationToken cancellationToken) =>
        {
            var response = await createBookingHandler.Handle(request, cancellationToken);

            return Results.Created($"/bookings/{response.Id}", response);

        })
        .RequireAuthorization("Customer")
        .Produces<CreateBookingResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);
    }
}
