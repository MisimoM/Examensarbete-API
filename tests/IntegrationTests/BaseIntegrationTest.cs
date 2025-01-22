using Microsoft.Extensions.DependencyInjection;
using Modules.Bookings.Data;
using Modules.Listings.Data;
using Modules.Users.Common.Helpers;
using Modules.Users.Data;

namespace IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly UserDbContext UserDbContext;
    protected readonly ListingDbContext ListingDbContext;
    protected readonly BookingDbContext BookingDbContext;
    protected readonly IPasswordHasher PasswordHasher;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        UserDbContext = _scope.ServiceProvider.GetRequiredService<UserDbContext>();
        ListingDbContext = _scope.ServiceProvider.GetRequiredService<ListingDbContext>();
        BookingDbContext = _scope.ServiceProvider.GetRequiredService<BookingDbContext>();

        PasswordHasher = _scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    }
    public void Dispose()
    {
        _scope?.Dispose();
        UserDbContext?.Dispose();
        ListingDbContext?.Dispose();
        BookingDbContext?.Dispose();
    }
}
