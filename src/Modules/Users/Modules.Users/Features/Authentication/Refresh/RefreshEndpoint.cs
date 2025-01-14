using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Users.Features.Authentication.Refresh;

internal class RefreshEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/auth/refresh", async (HttpContext httpContext, RefreshHandler handler) =>
        {
            var response = await handler.Handle(httpContext);

            return Results.Ok(response);
        });
    }
}
