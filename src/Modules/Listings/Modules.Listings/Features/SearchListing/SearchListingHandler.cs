using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;

namespace Modules.Listings.Features.SearchListing;

internal class SearchListingHandler(ListingDbContext dbContext)
{
    private readonly ListingDbContext _dbContext = dbContext;

    public async Task <List<SearchListingResponse>> Handle(SearchListingRequest request)
    {
        var query = _dbContext.Listings.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.MainLocation))
            query = query.Where(l => l.MainLocation == request.MainLocation);

        if (!string.IsNullOrWhiteSpace(request.SubLocation))
            query = query.Where(l => l.SubLocation == request.SubLocation);

        if (!string.IsNullOrWhiteSpace(request.AccommodationType))
            query = query.Where(l => l.AccommodationType == request.AccommodationType);

        var results = await query
            .Select(l => new SearchListingResponse
            (
                l.Id,
                l.Title,
                l.AccommodationType,
                l.MainLocation,
                l.SubLocation,
                l.Price,
                l.AvailableFrom,
                l.AvailableUntil,
                l.Images.Select(img => new ListingImageResponse(img.Url, img.AltText)).ToList()
            ))
            .ToListAsync();

        return results;
    }
}
