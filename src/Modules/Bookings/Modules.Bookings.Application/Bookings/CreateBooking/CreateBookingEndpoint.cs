using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Bookings.Application.Bookings.CreateBooking;

public class CreateBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("Bookings/Create", async (CreateBookingRequest request, CreateBookingHandler createBookingHandler, CancellationToken cancellationToken) =>
        {
            var response = await createBookingHandler.Handle(request, cancellationToken);
            return response;

        }).WithName(nameof(CreateBooking))
        .Produces<CreateBookingResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
