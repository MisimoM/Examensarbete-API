using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Listings.Features.SearchListing;

public class SearchListingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("listings/search", async (SearchListingRequest request, SearchListingHandler searchListingHandler, CancellationToken cancellationToken) =>
        {
            var response = await searchListingHandler.Handle(request, cancellationToken);
            return Results.Ok(response);

        }).WithName(nameof(SearchListing))
        .Produces<SearchListingResponse>(StatusCodes.Status200OK);
    }
}
