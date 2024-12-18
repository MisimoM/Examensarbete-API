using Modules.Listings;
using Modules.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserModule(builder.Configuration);
builder.Services.AddListingModule(builder.Configuration);

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

app.MapUserEndpoints();
app.MapListingEndpoints();

app.UseCors("AllowFrontend");

app.Run();
