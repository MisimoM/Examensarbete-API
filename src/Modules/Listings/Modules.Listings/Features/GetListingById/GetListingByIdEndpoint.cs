using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Listings.Features.GetListingById;

internal class GetListingByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("listings/getbyid/{id:guid}", async (Guid id, GetListingByIdHandler handler, CancellationToken cancellationToken) =>
        {
            var request = new GetListingByIdRequest(id);
            var response = await handler.Handle(request, cancellationToken);
            return Results.Ok(response);

        }).WithName(nameof(GetListingById))
        .Produces<GetListingByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
