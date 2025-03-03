using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Data;
using Modules.Bookings.Features.Bookings.CreateBooking;
using Modules.Bookings.Features.Payment.Klarna.CreateOrder;
using Modules.Bookings.Features.Payment.Klarna.GetOrder;
using Modules.Bookings.Features.Payment.Klarna.OrderCallback;
using Shared;
using System.Reflection;

namespace Modules.Bookings;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
           options.UseSqlServer(configuration.GetConnectionString("BookingDb")
       ));

        services.AddScoped<BookingDbContext>();

        services.AddHttpContextAccessor();

        services.AddValidatorsFromAssembly(Assembly.Load("Modules.Bookings"));

        services.AddScoped<CreateBookingHandler>();
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<GetOrderHandler>();
        services.AddScoped<OrderCallbackHandler>();

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

    public static void MigrateBooking(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var bookingContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
        bookingContext.Database.Migrate();
    }
}
