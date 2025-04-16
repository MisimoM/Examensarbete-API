using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;
using Shared.Contracts;
using Shared.Dtos;

namespace Modules.Listings.Communication;

internal class ListingService(ListingDbContext dbContext) : IListingService
{
    private readonly ListingDbContext _dbContext = dbContext;

    public async Task<ListingDto> GetByIdAsync(Guid id)
    {
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        if (listing is null)
            return null!;

        return new ListingDto(
            listing.Id,
            listing.Price
        );
    }
}
