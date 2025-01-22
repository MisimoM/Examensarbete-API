using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Data;
using Modules.Listings.Data;
using Modules.Users.Data;
using Testcontainers.MsSql;

namespace IntegrationTests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Admin123!")
        .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptorType = typeof(DbContextOptions<UserDbContext>);
                var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<UserDbContext>(options =>
                    options.UseSqlServer(_dbContainer.GetConnectionString()));

                services.AddDbContext<ListingDbContext>(options =>
                    options.UseSqlServer(_dbContainer.GetConnectionString()));

                services.AddDbContext<BookingDbContext>(options =>
                    options.UseSqlServer(_dbContainer.GetConnectionString()));
            });
        }

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
