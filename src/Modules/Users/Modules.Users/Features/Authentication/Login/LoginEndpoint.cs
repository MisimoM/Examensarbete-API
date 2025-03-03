﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Users.Features.Authentication.Login;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/auth/login", async (LoginRequest request, LoginHandler loginHandler, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var response = await loginHandler.Handle(request, httpContext, cancellationToken);
            return Results.Ok(response);

        }).WithName(nameof(Login))
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
