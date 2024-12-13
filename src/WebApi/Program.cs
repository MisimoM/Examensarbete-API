using Modules.Users;
using Modules.Users.Common.Helpers;
using Modules.Users.Data;
using Modules.Users.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserModule(builder.Configuration);

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

app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Name = "Marko",
            Email = "marko@email.com",
            Role = "Admin",
            Password = passwordHasher.Hash("marko123")
        });
        context.SaveChanges();
    }
}

app.Run();
