using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Users.Features.Users.CreateUser;

internal class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/users", async (CreateUserRequest request, CreateUserHandler createUserHandler, CancellationToken cancellationToken) =>
        {
            var response = await createUserHandler.Handle(request, cancellationToken);

            return Results.Created($"/users/{response.Id}", response);

        }).WithName(nameof(CreateUser))
        .Produces<CreateUserResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);
    }
}
