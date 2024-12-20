using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;
using Modules.Listings.Entities;

namespace Modules.Listings.Communication;

internal class ListingService(ListingDbContext dbContext) : IListingService
{
    private readonly ListingDbContext _dbContext = dbContext;

    public async Task<Listing> GetByIdAsync(Guid id)
    {
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        if (listing is null)
            throw new Exception($"Listing with ID {id} not found.");

        return listing;
    }
}
