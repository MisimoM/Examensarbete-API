using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Listings.Features.GetListing;

public class GetListingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("listings", async ([AsParameters] GetListingRequest request, GetListingHandler searchListingHandler, CancellationToken cancellationToken) =>
        {
            var response = await searchListingHandler.Handle(request, cancellationToken);
            return Results.Ok(response);

        }).WithName(nameof(GetListing))
        .Produces<GetListingResponse>(StatusCodes.Status200OK);
    }
}
