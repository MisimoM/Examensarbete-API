using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Listings.Features.SearchListing;

public class SearchListingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("Search", async (SearchListingRequest request, SearchListingHandler searchListingHandler) =>
        {
            var response = await searchListingHandler.Handle(request);
            return response;

        }).WithName(nameof(SearchListing))
        .Produces<SearchListingResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
