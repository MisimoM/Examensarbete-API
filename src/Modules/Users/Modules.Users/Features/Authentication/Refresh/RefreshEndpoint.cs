using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Users.Features.Authentication.Login;
using Shared;

namespace Modules.Users.Features.Authentication.Refresh;

internal class RefreshEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/auth/refresh", async (HttpContext httpContext, RefreshHandler handler, CancellationToken cancellationToken) =>
        {
            var response = await handler.Handle(httpContext, cancellationToken);
            return Results.Ok(response);

        }).WithName(nameof(Refresh))
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
