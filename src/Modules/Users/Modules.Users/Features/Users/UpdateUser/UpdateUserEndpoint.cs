using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Users.Features.Users.UpdateUser;

public class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("/users/{id:guid}", async (Guid id, UpdateUserRequest request, UpdateUserHandler updateUserHandler, CancellationToken cancellationToken) =>
        {
            var response = await updateUserHandler.Handle(id, request, cancellationToken);

            return Results.Ok(response);

        }).WithName(nameof(UpdateUser))
        .Produces<UpdateUserResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);
    }
}
