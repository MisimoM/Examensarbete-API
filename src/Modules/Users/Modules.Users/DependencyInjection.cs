using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Common.Helpers;
using Modules.Users.Common.Identity;
using Modules.Users.Data;
using Modules.Users.Features.Authentication.Login;
using FluentValidation;
using System.Reflection;
using Shared;
using Modules.Users.Features.Authentication.Refresh;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Modules.Users.Data.Seed;
using Modules.Users.Features.Users.CreateUser;
using Modules.Users.Features.Users.UpdateUser;
using Modules.Users.Features.Users.DeleteUser;
using Modules.Users.Communication;
using Modules.Users.Common.Enums;
using Shared.Contracts;

namespace Modules.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUserModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("UserDb")
        ));

        services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };
                });

        services.AddAuthorizationBuilder()
            .AddPolicy("Customer", policy => policy.RequireRole(UserRole.Customer.ToString(), UserRole.Host.ToString(), UserRole.Admin.ToString()))
            .AddPolicy("Host", policy => policy.RequireRole(UserRole.Host.ToString(), UserRole.Admin.ToString()))
            .AddPolicy("Admin", policy => policy.RequireRole(UserRole.Admin.ToString()));

        services.AddValidatorsFromAssembly(Assembly.Load("Modules.Users"));

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshHandler>();

        services.AddScoped<CreateUserHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<DeleteUserHandler>();

        return services;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
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

    public static void MigrateAndSeedUser(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var userContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        userContext.Database.Migrate();
        UserSeeder.Seed(userContext, passwordHasher);
    }
}
