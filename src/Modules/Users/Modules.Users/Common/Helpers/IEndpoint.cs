using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Modules.Users.Common.Helpers;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder builder);
}
