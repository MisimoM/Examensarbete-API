using Microsoft.Extensions.DependencyInjection;
using Shared.ExceptionHandlers;
using Shared.Helpers;

namespace Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContextHelper, UserContextHelper>();

        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<ConflictExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
