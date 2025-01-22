using Modules.Bookings;
using Modules.Listings;
using Modules.Users;
using Scalar.AspNetCore;
using Serilog;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSharedServices();
builder.Services.AddUserModule(builder.Configuration);
builder.Services.AddListingModule(builder.Configuration);
builder.Services.AddBookingModule(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateAndSeedUser();
    app.MigrateAndSeedListing();
    app.MigrateBooking();

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapListingEndpoints();
app.MapBookingEndpoints();

app.Run();

public partial class Program { }
