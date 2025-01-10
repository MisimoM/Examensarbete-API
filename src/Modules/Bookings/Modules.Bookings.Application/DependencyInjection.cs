using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Application.Bookings.CreateBooking;
using Shared;
using System.Reflection;

namespace Modules.Bookings.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBookingModuleApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddValidatorsFromAssembly(Assembly.Load("Modules.Bookings.Application"));

            services.AddScoped<CreateBookingHandler>();


            return services;
        }

        public static WebApplication MapBookingEndpoints(this WebApplication app)
        {

            var endpointTypes = typeof(DependencyInjection).Assembly.GetTypes()
                .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var endpointType in endpointTypes)
            {
                var endpoint = (IEndpoint)Activator.CreateInstance(endpointType)!;
                endpoint.MapEndpoint(app);
            }

            return app;
        }
    }
}
