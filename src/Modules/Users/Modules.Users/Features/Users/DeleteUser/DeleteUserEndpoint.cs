using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared;

namespace Modules.Users.Features.Users.DeleteUser;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/users/{id:guid}", async (Guid id, DeleteUserHandler deleteUserHandler, CancellationToken cancellationToken) =>
        {
            var request = new DeleteUserRequest(id);
            var response = await deleteUserHandler.Handle(request, cancellationToken);

            return Results.Ok(response);

        }).WithName(nameof(DeleteUser))
        .Produces<DeleteUserResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

    }
}
