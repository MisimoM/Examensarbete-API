using Modules.Bookings.Application;
using Modules.Bookings.Infrastructure;
using Modules.Listings;
using Modules.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserModule(builder.Configuration);
builder.Services.AddListingModule(builder.Configuration);
builder.Services.AddBookingModuleInfrastructure(builder.Configuration);
builder.Services.AddBookingModuleApplication(builder.Configuration);

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

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapListingEndpoints();
app.MapBookingEndpoints();

app.Run();
