using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Listings.Features.CreateListing;

public class CreateListingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/listings", async (CreateListingRequest request, CreateListingHandler handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(request, cancellationToken);

            return Results.Created($"/listings/{result.Id}", result);

        })
        .RequireAuthorization("Host")
        .WithName(nameof(CreateListing))
        .Produces<CreateListingResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
