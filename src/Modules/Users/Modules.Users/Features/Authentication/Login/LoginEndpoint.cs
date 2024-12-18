using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Users.Features.Authentication.Login;

internal class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("login", async (LoginRequest request, LoginHandler loginHandler, HttpContext httpContext) =>
        {
            var response = await loginHandler.Handle(request, httpContext);
            return response;

        }).WithName(nameof(Login))
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
